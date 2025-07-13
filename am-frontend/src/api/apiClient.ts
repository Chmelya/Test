import axios from 'axios';

// TODO: config
const apiClient = axios.create({
	baseURL: 'https://localhost:7196/api',
});

export default apiClient;
