import axios from 'axios';
import Cookies from 'js-cookie';

const API_URL = "https://localhost:6061/api/account";

const cookieOptions = {
  expires: 7,
  secure: true, 
  sameSite: 'Strict',
};

export const isAuthorized = () => {
  const badResults = [null, undefined];
  const response = Cookies.get('user');
  return badResults.some(e => e === response) ? Boolean(false) : Boolean(true); 
};

export const login = async (emailOrUsername, password) => {
  const response = await axios.post(`${API_URL}/Login`, {
    emailOrUsername,
    password,
  });

  if (response.data.token && response.data.refreshToken && response.data.user) {
    Cookies.set('accessToken', response.data.token, cookieOptions);
    Cookies.set('refreshToken', response.data.refreshToken, cookieOptions);
    Cookies.set('user', JSON.stringify(response.data.user), cookieOptions);
  }

  return response.data;
};

export const register = async (userData) => {
  const response = await axios.post(`${API_URL}/Register`, userData);

  if (response.data.token && response.data.refreshToken && response.data.newUser) {
    Cookies.set('accessToken', response.data.token, cookieOptions);
    Cookies.set('refreshToken', response.data.refreshToken, cookieOptions);
    Cookies.set('user', JSON.stringify(response.data.newUser), cookieOptions);
  }

  return response.data;
};

export const logout = async () => {
  Cookies.remove('accessToken');
  Cookies.remove('refreshToken');
  Cookies.remove('user');
  await axios.post(`${API_URL}/Logout`);
};

export const getCurrentToken = () => {
  return Cookies.get('accessToken');
};

export const getCurrentUser = () => {
  const user = Cookies.get('user');
  return user ? JSON.parse(user) : null;
};

export const refreshToken = async () => {
  const refreshToken = Cookies.get('refreshToken');
  const token = Cookies.get('accessToken');

  const response = await axios.post(`${API_URL}/refresh-token`, {
    token: token,
    refreshToken: refreshToken,
  });

  if (response.data.token && response.data.refreshToken) {
    Cookies.set('accessToken', response.data.token, cookieOptions);
    Cookies.set('refreshToken', response.data.refreshToken, cookieOptions);
  }

  return response.data;
};
