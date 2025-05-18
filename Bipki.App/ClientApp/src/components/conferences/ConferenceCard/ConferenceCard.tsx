import { Box, Button, Typography, styled } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { Conference } from "types/Conference";
import DeleteIcon from '@mui/icons-material/Delete';

const CardContainer = styled(Box)({
  width: "311px",
  margin: "0 auto",
  padding: "10px",
  boxSizing: "border-box",
  borderRadius: "4px",
  border: "1px solid #A980E0",
});

interface ConferenceCardProps {
  conference: Conference;
  onDelete?: (conference: Conference) => void;
}

export const ConferenceCard = ({
  conference,
  onDelete,
}: ConferenceCardProps) => {
  const navigate = useNavigate();

  const handleCardClick = (e: React.MouseEvent) => {
    if ((e.target as HTMLElement).closest("button")) {
      return;
    }
    navigate(`/admin/conferences/${conference.id}`);
  };

  const formatDate = (date: Date) => {
    return new Date(date).toLocaleDateString("ru-RU", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
    });
  };

  return (
    <CardContainer onClick={handleCardClick} sx={{ cursor: "pointer" }}>
      <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start", mb: 1 }}>
        <Typography variant="h6" sx={{ fontSize: "16px", fontWeight: 600 }}>
          {conference.title}
        </Typography>
        <Button
          variant="text"
          size="small"
          onClick={(e) => {
            e.stopPropagation();
            onDelete?.(conference);
          }}
          sx={{
            minWidth: "auto",
            padding: "4px",
          }}
        >
          <DeleteIcon fontSize="small" />
        </Button>
      </Box>
      <Box sx={{ display: "flex", flexDirection: "column", gap: "4px" }}>
        <Typography variant="body2" color="text.secondary">
          {formatDate(conference.startDate)} - {formatDate(conference.endDate)}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          Участников: {conference.participantsCount}
        </Typography>
      </Box>
    </CardContainer>
  );
}; 