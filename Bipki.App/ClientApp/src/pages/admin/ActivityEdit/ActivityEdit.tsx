import { Box, Typography, TextField, Button, Stack, Checkbox, FormControlLabel, Autocomplete, createFilterOptions } from "@mui/material";
import { useParams, useNavigate } from "react-router-dom";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { getActivity, updateActivity, createActivity, PatchActivityRequest, CreateActivityRequest } from "api/activityApi";
import { ActivityType } from "types/Activity";
import { useState, useEffect, useCallback } from "react";
import { Loader } from "components/common/Loader/Loader";

const typeOptions = ["Мастер-класс", "Лекция"];
const filter = createFilterOptions<string>();

export const ActivityEdit = () => {
  const { conferenceId, activityId } = useParams();
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const isCreate = activityId === "create";
  const [form, setForm] = useState({
    title: "",
    startDateTime: "",
    endDateTime: "",
    description: "",
    typeLabel: "",
    hasSeatsLimit: false,
    totalSeats: "",
  });
  const [errors, setErrors] = useState<Record<string, string>>({});

  const { data: activity, isLoading } = useQuery({
    queryKey: ["activity", activityId],
    queryFn: () => getActivity(activityId!),
    enabled: !!activityId && !isCreate,
  });

  useEffect(() => {
    if (activity) {
      setForm({
        title: activity.title || "",
        startDateTime: activity.startDateTime ? new Date(activity.startDateTime).toISOString().slice(0, 16) : "",
        endDateTime: activity.endDateTime ? new Date(activity.endDateTime).toISOString().slice(0, 16) : "",
        description: activity.description || "",
        typeLabel: activity.typeLabel || "",
        hasSeatsLimit: activity.type === ActivityType.Workshop,
        totalSeats: activity.type === ActivityType.Workshop ? String((activity as any).totalSeats || "") : "",
      });
    }
  }, [activity]);

  const updateMutation = useMutation({
    mutationFn: (data: PatchActivityRequest) => updateActivity(conferenceId!, activityId!, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["activity", activityId] });
      queryClient.invalidateQueries({ queryKey: ["conferenceActivities", conferenceId] });
      navigate(`/admin/conferences/${conferenceId}/activities`);
    },
  });

  const createMutation = useMutation({
    mutationFn: (data: CreateActivityRequest) => createActivity(conferenceId!, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["conferenceActivities", conferenceId] });
      navigate(`/admin/conferences/${conferenceId}/activities`);
    },
  });

  const validateForm = useCallback(() => {
    const newErrors: Record<string, string> = {};

    if (!form.title.trim()) {
      newErrors.title = "Название активности обязательно";
    }

    if (!form.startDateTime) {
      newErrors.startDateTime = "Дата начала обязательна";
    }

    if (!form.endDateTime) {
      newErrors.endDateTime = "Дата окончания обязательна";
    }

    if (!form.typeLabel.trim()) {
      newErrors.typeLabel = "Тип активности обязателен";
    }

    if (!form.description.trim()) {
      newErrors.description = "Описание активности обязательно";
    }

    if (form.hasSeatsLimit && !form.totalSeats) {
      newErrors.totalSeats = "Укажите количество мест";
    }

    if (form.hasSeatsLimit && form.totalSeats && parseInt(form.totalSeats) <= 0) {
      newErrors.totalSeats = "Количество мест должно быть больше 0";
    }

    if (form.startDateTime && form.endDateTime) {
      const start = new Date(form.startDateTime);
      const end = new Date(form.endDateTime);

      if (start >= end) {
        newErrors.endDateTime = "Дата окончания должна быть позже даты начала";
      }
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  }, [form, setErrors]);

  useEffect(() => {
    if (form) {
      validateForm();
    }
  }, [form, validateForm]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: value }));
  };

  const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm(prev => ({ 
      ...prev, 
      hasSeatsLimit: e.target.checked,
      totalSeats: e.target.checked ? prev.totalSeats : "",
    }));
  };

  const handleCancel = () => {
    navigate(`/admin/conferences/${conferenceId}/activities`);
  };

  const handleSave = () => {
    if (!validateForm()) {
      return;
    }

    const data = {
      title: form.title,
      startDateTime: new Date(form.startDateTime),
      endDateTime: new Date(form.endDateTime),
      description: form.description,
      typeLabel: form.typeLabel,
      type: form.hasSeatsLimit ? ActivityType.Workshop : ActivityType.Lecture,
    } as CreateActivityRequest;

    if (form.hasSeatsLimit && form.totalSeats) {
      data.totalSeats = parseInt(form.totalSeats);
    }

    if (isCreate) {
      createMutation.mutate(data);
    } else {
      updateMutation.mutate(data);
    }
  };

  if (isLoading && !isCreate) {
    return <Loader />;
  }

  if (!activity && !isCreate) {
    return <div>Активность не найдена</div>;
  }

  return (
    <Box sx={{ padding: "16px", maxWidth: 500, margin: "0 auto" }}>
      <Typography variant="h5" sx={{ mb: 2 }}>
        {isCreate ? "Создание мероприятия" : "Редактирование активности"}
      </Typography>
      <Stack spacing={2}>
        <TextField
          label="Название активности"
          name="title"
          value={form.title}
          onChange={handleChange}
          variant="standard"
          fullWidth
          required
          error={!!errors.title}
          helperText={errors.title}
        />
        <TextField
          label="Дата начала"
          name="startDateTime"
          type="datetime-local"
          value={form.startDateTime}
          onChange={handleChange}
          variant="standard"
          fullWidth
          required
          error={!!errors.startDateTime}
          helperText={errors.startDateTime}
          slotProps={{
            inputLabel: {
              shrink: true,
            },
          }}
        />
        <TextField
          label="Дата окончания"
          name="endDateTime"
          type="datetime-local"
          value={form.endDateTime}
          onChange={handleChange}
          variant="standard"
          fullWidth
          required
          error={!!errors.endDateTime}
          helperText={errors.endDateTime}
          slotProps={{
            inputLabel: {
              shrink: true,
            },
          }}
        />
        <Autocomplete
          freeSolo
          options={typeOptions}
          filterOptions={(options, state) => {
            const filtered = filter(options, state);
            if (state.inputValue !== "" && !options.includes(state.inputValue)) {
              filtered.push(state.inputValue);
            }
            return filtered;
          }}
          value={form.typeLabel}
          onChange={(_, newValue) => setForm(prev => ({ ...prev, typeLabel: newValue || "" }))}
          renderInput={(params) => (
            <TextField
              {...params}
              label="Тип активности"
              variant="standard"
              fullWidth
              required
              error={!!errors.typeLabel}
              helperText={errors.typeLabel}
            />
          )}
        />
        <TextField
          label="Описание активности"
          name="description"
          value={form.description}
          onChange={handleChange}
          variant="standard"
          fullWidth
          multiline
          minRows={3}
          required
          error={!!errors.description}
          helperText={errors.description}
        />
        <FormControlLabel
          control={
            <Checkbox
              checked={form.hasSeatsLimit}
              onChange={handleCheckboxChange}
              sx={{
                color: "#A980E0",
                "&.Mui-checked": {
                  color: "#A980E0",
                },
              }}
            />
          }
          label="Ограничение мест"
        />
        {form.hasSeatsLimit && (
          <TextField
            label="Количество мест"
            name="totalSeats"
            type="number"
            value={form.totalSeats}
            onChange={handleChange}
            variant="standard"
            fullWidth
            required
            error={!!errors.totalSeats}
            helperText={errors.totalSeats}
          />
        )}
        <Stack direction="row" spacing={2} justifyContent="space-between">
          <Button 
            variant="outlined" 
            color="secondary" 
            onClick={handleCancel} 
            disabled={updateMutation.isPending || createMutation.isPending}
            sx={{
              backgroundColor: "#6C6C6C"
            }}
            fullWidth
          >
            Отменить
          </Button>
          <Button 
            variant="contained" 
            color="primary" 
            onClick={handleSave} 
            disabled={updateMutation.isPending || createMutation.isPending}
            fullWidth
          >
            {isCreate ? "Создать" : "Сохранить"}
          </Button>
        </Stack>
      </Stack>
    </Box>
  );
}; 