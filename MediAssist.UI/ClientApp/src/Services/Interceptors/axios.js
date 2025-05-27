import axios from 'axios';

// Create an Axios instance
const axiosInstance = axios.create({
  baseURL: '/',
  headers: {
    'Content-Type': 'application/json',
  }, 
});

// Request Interceptor
axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('authToken'); 
    
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }else {
      console.warn('Token is missing. Please log in again.');
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default axiosInstance;
