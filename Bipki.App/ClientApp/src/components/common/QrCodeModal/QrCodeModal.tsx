import { Modal, Box, IconButton, CircularProgress } from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import { useQuery } from '@tanstack/react-query';
import { getConferenceQrCode } from '../../../api/conferenceApi';

interface QrCodeModalProps {
    open: boolean;
    onClose: () => void;
    conferenceId: string;
}

const style = {
    position: 'absolute' as 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 400,
    bgcolor: 'background.paper',
    borderRadius: 2,
    boxShadow: 24,
    p: 4,
};

export const QrCodeModal = ({ open, onClose, conferenceId }: QrCodeModalProps) => {
    const { data: qrCode, isLoading } = useQuery({
        queryKey: ["conferenceQrCode", conferenceId],
        queryFn: () => getConferenceQrCode(conferenceId),
        enabled: open,
    });

    return (
        <Modal
            open={open}
            onClose={onClose}
            aria-labelledby="qr-code-modal"
        >
            <Box sx={style}>
                <IconButton
                    onClick={onClose}
                    sx={{ position: 'absolute', right: 8, top: 8 }}
                >
                    <CloseIcon />
                </IconButton>
                {isLoading ? (
                    <Box sx={{ display: 'flex', justifyContent: 'center', p: 4 }}>
                        <CircularProgress />
                    </Box>
                ) : qrCode && (
                    <img
                        src={`data:image/png;base64,${qrCode}`}
                        alt="QR Code"
                        style={{ width: '100%', height: 'auto' }}
                    />
                )}
            </Box>
        </Modal>
    );
}; 