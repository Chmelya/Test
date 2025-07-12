import axios from 'axios';

const apiClient = axios.create({
	baseURL: 'https://localhost:7196/api',
});

export default apiClient;
