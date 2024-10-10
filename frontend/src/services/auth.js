import axios from 'axios';

const API_URL = 'https://localhost:5286/api/Account';

export const login = async (emailOrUsername, password) => {
  const response = await axios.post(`${API_URL}/Login`, {
    emailOrUsername,
    password
  });
  
  if (response.data.token && response.data.refreshToken) {
    // Сохраняем токены в localStorage
    localStorage.setItem('accessToken', response.data.token);
    localStorage.setItem('refreshToken', response.data.refreshToken);
  }

  return response.data;
};

export const register = async (userData) => {
  const response = await axios.post(`${API_URL}/Register`, userData);
  
  if (response.data.token && response.data.refreshToken) {
    localStorage.setItem('accessToken', response.data.token);
    localStorage.setItem('refreshToken', response.data.refreshToken);
  }

  return response.data;
};

export const logout = async () => {
  localStorage.removeItem('accessToken');
  localStorage.removeItem('refreshToken');
  await axios.post(`${API_URL}/Logout`);
};

export const getCurrentToken = () => {
  return localStorage.getItem('accessToken');
};

export const refreshToken = async () => {
  const refreshToken = localStorage.getItem('refreshToken');
  const token = localStorage.getItem('accessToken');

  const response = await axios.post(`${API_URL}/refresh-token`, {
    token: token,
    refreshToken: refreshToken
  });

  if (response.data.token && response.data.refreshToken) {
    localStorage.setItem('accessToken', response.data.token);
    localStorage.setItem('refreshToken', response.data.refreshToken);
  }

  return response.data;
};
