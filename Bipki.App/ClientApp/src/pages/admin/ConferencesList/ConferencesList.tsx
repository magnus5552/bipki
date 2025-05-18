import { Box, Typography, Button, Stack, Dialog, DialogTitle, DialogContent, DialogActions } from "@mui/material";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { getConferences, deleteConference } from "api/conferenceApi";
import { Loader } from "components/common/Loader/Loader";
import { ConferenceCard } from "components/conferences/ConferenceCard/ConferenceCard";
import { useNavigate } from "react-router-dom";
import AddIcon from "@mui/icons-material/Add";
import { Conference } from "types/Conference";
import { useState } from "react";

export const ConferencesList = () => {
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [conferenceToDelete, setConferenceToDelete] = useState<Conference | null>(null);

  const {
    data: conferences,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["conferences"],
    queryFn: getConferences,
  });

  const deleteMutation = useMutation({
    mutationFn: (conferenceId: string) => deleteConference(conferenceId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["conferences"] });
      setDeleteDialogOpen(false);
      setConferenceToDelete(null);
    },
  });

  const handleDelete = (conference: Conference) => {
    setConferenceToDelete(conference);
    setDeleteDialogOpen(true);
  };

  const handleConfirmDelete = () => {
    if (conferenceToDelete) {
      deleteMutation.mutate(conferenceToDelete.id);
    }
  };

  const handleCloseDeleteDialog = () => {
    setDeleteDialogOpen(false);
    setConferenceToDelete(null);
  };

  if (isLoading) {
    return <Loader />;
  }

  if (error) {
    return <div>Ошибка при загрузке конференций</div>;
  }

  return (
    <>
      <Stack
        direction="column"
        justifyContent="space-between"
        alignItems="center"
        spacing={2}
        sx={{ paddingBottom: "16px" }}
      >
        <Box sx={{ display: "flex", flexDirection: "column", gap: "10px" }}>
          {conferences?.map((conference) => (
            <ConferenceCard
              key={conference.id}
              conference={conference}
              onDelete={handleDelete}
            />
          ))}
        </Box>
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={() => navigate("/admin/conferences/create/edit")}
          sx={{
            width: "311px",
            backgroundColor: "#A980E0",
            "&:hover": {
              backgroundColor: "#CEAAFF",
            },
          }}
        >
          Добавить конференцию
        </Button>
      </Stack>

      <Dialog
        open={deleteDialogOpen}
        onClose={handleCloseDeleteDialog}
        maxWidth="xs"
        fullWidth
      >
        <DialogTitle>Подтверждение удаления</DialogTitle>
        <DialogContent>
          <Typography>
            Вы уверены, что хотите удалить конференцию "{conferenceToDelete?.name}"?
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button
            onClick={handleCloseDeleteDialog}
            color="primary"
            disabled={deleteMutation.isPending}
          >
            Отмена
          </Button>
          <Button
            onClick={handleConfirmDelete}
            color="error"
            variant="contained"
            disabled={deleteMutation.isPending}
          >
            Удалить
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};
