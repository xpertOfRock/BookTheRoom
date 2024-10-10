import axios from 'axios';

const API_URL = 'https://localhost:5286/api/Account';


export const login = async (emailOrUsername, password) => {
  const response = await axios.post(`${API_URL}/Login`, {
    emailOrUsername,
    password,
  });

  if (response.data.token && response.data.refreshToken && response.data.user) {
    localStorage.setItem('accessToken', response.data.token);
    localStorage.setItem('refreshToken', response.data.refreshToken);
    localStorage.setItem('user', JSON.stringify(response.data.user));
  }

  return response.data;
};


export const register = async (userData) => {
  const response = await axios.post(`${API_URL}/Register`, userData);

  if (response.data.token && response.data.refreshToken && response.data.newUser) {
    localStorage.setItem('accessToken', response.data.token);
    localStorage.setItem('refreshToken', response.data.refreshToken);
    localStorage.setItem('user', JSON.stringify(response.data.newUser));
  }

  return response.data;
};


export const logout = async () => {
  localStorage.removeItem('accessToken');
  localStorage.removeItem('refreshToken');
  localStorage.removeItem('user');
  await axios.post(`${API_URL}/Logout`);
};

// Получение текущего токена
export const getCurrentToken = () => {
  return localStorage.getItem('accessToken');
};


export const getCurrentUser = () => {
  return JSON.parse(localStorage.getItem('user'));
};


export const refreshToken = async () => {
  const refreshToken = localStorage.getItem('refreshToken');
  const token = localStorage.getItem('accessToken');

  const response = await axios.post(`${API_URL}/refresh-token`, {
    token: token,
    refreshToken: refreshToken,
  });

  if (response.data.token && response.data.refreshToken) {
    localStorage.setItem('accessToken', response.data.token);
    localStorage.setItem('refreshToken', response.data.refreshToken);
  }

  return response.data;
};
