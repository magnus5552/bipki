import { useQuery } from "@tanstack/react-query";
import { getUser } from "api/authApi";

export function useUser() {
  const { data, isLoading, error } = useQuery({
    queryKey: ["user"],
    queryFn: getUser,
  });
  return { user: data, isLoading, error };
}