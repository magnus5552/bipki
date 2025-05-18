import { useMutation, useQueryClient } from '@tanstack/react-query';
import { registerForActivity, unregisterFromActivity, confirmRegistration } from '../api/activityApi';
import { RegistrationStatus } from '../types/Activity';

export const useActivityRegistration = (activityId: string) => {
    const queryClient = useQueryClient();

    const registerMutation = useMutation({
        mutationFn: () => registerForActivity(activityId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['activities'] });
            queryClient.invalidateQueries({ queryKey: ['activity', activityId] });
        },
    });

    const unregisterMutation = useMutation({
        mutationFn: () => unregisterFromActivity(activityId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['activities'] });
            queryClient.invalidateQueries({ queryKey: ['activity', activityId] });
        },
    });

    const confirmMutation = useMutation({
        mutationFn: () => confirmRegistration(activityId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['activities'] });
            queryClient.invalidateQueries({ queryKey: ['activity', activityId] });
        },
    });

    const handleAction = (registrationStatus: RegistrationStatus) => {
        switch (registrationStatus) {
            case RegistrationStatus.NotRegistered:
                registerMutation.mutate();
                break;
            case RegistrationStatus.Registered:
            case RegistrationStatus.WaitingList:
                unregisterMutation.mutate();
                break;
            case RegistrationStatus.PendingConfirmation:
                confirmMutation.mutate();
                break;
        }
    };

    return {
        handleAction,
        isLoading: registerMutation.isPending || unregisterMutation.isPending || confirmMutation.isPending,
    };
}; 