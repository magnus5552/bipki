import { Modal, Box, IconButton } from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';

interface QrCodeModalProps {
    open: boolean;
    onClose: () => void;
    qrCodeData: Uint8Array | null;
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

export const QrCodeModal = ({ open, onClose, qrCodeData }: QrCodeModalProps) => {
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
                {qrCodeData && (
                    <img
                        src={`data:image/png;base64,${Buffer.from(qrCodeData).toString('base64')}`}
                        alt="QR Code"
                        style={{ width: '100%', height: 'auto' }}
                    />
                )}
            </Box>
        </Modal>
    );
}; 