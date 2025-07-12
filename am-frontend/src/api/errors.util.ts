import { isAxiosError } from 'axios';

export const getApiErrorMessage = (e: Error) => {
	if (isAxiosError(e) && e.response?.data?.detail) {
		return e.response.data.detail;
	}

	return e?.message || 'Something went wrong';
};
