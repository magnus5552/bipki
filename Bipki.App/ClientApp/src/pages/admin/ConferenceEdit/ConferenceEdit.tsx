import { Box, Typography, TextField, Button, Stack } from "@mui/material";
import { useParams, useNavigate } from "react-router-dom";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { getConference, updateConference, createConference, PatchConferenceRequest, CreateConferenceRequest } from "api/conferenceApi";
import { useState, useEffect, useCallback } from "react";
import { Loader } from "components/common/Loader/Loader";

export const ConferenceEdit = () => {
  const { conferenceId } = useParams();
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const isCreate = conferenceId === "create";

  const [form, setForm] = useState({
    name: "",
    startDate: "",
    endDate: "",
    location: "",
    description: "",
    plan: "",
  });
  const [errors, setErrors] = useState<Record<string, string>>({});

  const { data: conference, isLoading } = useQuery({
    queryKey: ["conference", conferenceId],
    queryFn: () => getConference(conferenceId!),
    enabled: !!conferenceId && !isCreate,
  });

  useEffect(() => {
    if (conference) {
      setForm({
        name: conference.title || "",
        startDate: conference.startDate
          ? new Date(conference.startDate).toISOString().slice(0, 16)
          : "",
        endDate: conference.endDate
          ? new Date(conference.endDate).toISOString().slice(0, 16)
          : "",
        location: conference.address || "",
        description: conference.description || "",
        plan: conference.plan || "",
      });
    }
  }, [conference]);

  const updateMutation = useMutation({
    mutationFn: (data: PatchConferenceRequest) =>
      updateConference(conferenceId!, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["conference"] });
      queryClient.invalidateQueries({ queryKey: ["conferences"] });
      navigate(`/admin/conferences/${conferenceId}`);
    },
  });

  const createMutation = useMutation({
    mutationFn: (data: CreateConferenceRequest) => createConference(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["conferences"] });
      navigate("/admin/conferences");
    },
  });

  const validateForm = useCallback(() => {
    const newErrors: Record<string, string> = {};

    if (!form.name.trim()) {
      newErrors.name = "Название конференции обязательно";
    }

    if (!form.startDate) {
      newErrors.startDate = "Дата начала обязательна";
    }

    if (!form.endDate) {
      newErrors.endDate = "Дата окончания обязательна";
    }

    if (!form.location.trim()) {
      newErrors.location = "Место проведения обязательно";
    }

    if (!form.description.trim()) {
      newErrors.description = "Описание конференции обязательно";
    }

    if (form.startDate && form.endDate) {
      const start = new Date(form.startDate);
      const end = new Date(form.endDate);

      if (start >= end) {
        newErrors.endDate = "Дата окончания должна быть позже даты начала";
      }
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  }, [form]);

  useEffect(() => {
    if (form) {
      validateForm();
    }
  }, [form, validateForm]);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleCancel = () => {
    navigate("/admin/conferences");
  };

  const handleSave = () => {
    if (!validateForm()) {
      return;
    }

    const data = {
      name: form.name,
      startDate: new Date(form.startDate),
      endDate: new Date(form.endDate),
      location: form.location,
      description: form.description,
      plan: form.plan,
    };

    if (isCreate) {
      createMutation.mutate(data);
    } else {
      updateMutation.mutate(data);
    }
  };

  if (isLoading && !isCreate) {
    return <Loader />;
  }

  if (!conference && !isCreate) {
    return <div>Конференция не найдена</div>;
  }

  return (
    <Box sx={{ padding: "16px", maxWidth: 500, margin: "0 auto" }}>
      <Typography variant="h5" sx={{ mb: 2 }}>
        {isCreate ? "Создание конференции" : "Редактирование конференции"}
      </Typography>
      <Stack spacing={2}>
        <TextField
          label="Название конференции"
          name="name"
          value={form.name}
          onChange={handleChange}
          variant="standard"
          fullWidth
          required
          error={!!errors.name}
          helperText={errors.name}
        />
        <TextField
          label="Дата начала"
          name="startDate"
          type="datetime-local"
          value={form.startDate}
          onChange={handleChange}
          variant="standard"
          fullWidth
          required
          error={!!errors.startDate}
          helperText={errors.startDate}
          slotProps={{
            inputLabel: {
              shrink: true,
            },
          }}
        />
        <TextField
          label="Дата окончания"
          name="endDate"
          type="datetime-local"
          value={form.endDate}
          onChange={handleChange}
          variant="standard"
          fullWidth
          required
          error={!!errors.endDate}
          helperText={errors.endDate}
          slotProps={{
            inputLabel: {
              shrink: true,
            },
          }}
        />
        <TextField
          label="Место проведения"
          name="location"
          value={form.location}
          onChange={handleChange}
          variant="standard"
          fullWidth
          required
          error={!!errors.location}
          helperText={errors.location}
        />
        <TextField
          label="Описание конференции"
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
        <TextField
          label="Ссылка на карту"
          name="plan"
          value={form.plan}
          onChange={handleChange}
          variant="standard"
          fullWidth
          placeholder="https://..."
          helperText="Необязательное поле"
        />
        <Stack direction="row" spacing={2} justifyContent="space-between">
          <Button
            variant="outlined"
            color="secondary"
            sx={{
              backgroundColor: "#6C6C6C"
            }}
            onClick={handleCancel}
            disabled={updateMutation.isPending || createMutation.isPending}
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

