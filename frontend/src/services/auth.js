import axios from 'axios';
import Cookies from 'js-cookie';

axios.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (error.response && error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      try {
        const refreshedData = await refreshToken();
        const newAccessToken = refreshedData.token;

        Cookies.set('accessToken', newAccessToken, {
          expires: 7,
          secure: true,
          sameSite: 'Strict',
        });

        axios.defaults.headers.common['Authorization'] = `Bearer ${newAccessToken}`;
        originalRequest.headers['Authorization'] = `Bearer ${newAccessToken}`;

        return axios(originalRequest);
      } catch (refreshError) {
        console.error('Refresh token failed:', refreshError.response ? refreshError.response.data : refreshError.message);
        Cookies.remove('accessToken');
        Cookies.remove('refreshToken');
        Cookies.remove('user');
        window.location.href = '/login';
        return Promise.reject(refreshError);
      }
    }

    return Promise.reject(error);
  }
);

const API_URL = "https://localhost:6061/api/account";

const cookieOptions = {
  expires: 7,
  secure: true,
  sameSite: 'Strict',
};

export const isAuthorized = () => {
  const user = Cookies.get('user');
  return Boolean(user);
};

export const getUserById = async (userId) => {
  try{
    const token = getCurrentToken();
    const response = await axios.get(`${API_URL}/${userId}`,
      {
        headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    return response.data;
  } catch (error) {
    console.error(error);
  }
}

export const login = async (emailOrUsername, password) => {
  try {
    const response = await axios.post(`${API_URL}/Login`, { emailOrUsername, password });

    if (response.data.token && response.data.refreshToken && response.data.user) {
      Cookies.set('accessToken', response.data.token, cookieOptions);
      Cookies.set('refreshToken', response.data.refreshToken, cookieOptions);
      Cookies.set('user', JSON.stringify(response.data.user), cookieOptions);
    }

    return response.data;
  } catch (error) {
    console.error('Login failed:', error.response?.data || error.message);
    throw new Error(error.response?.data?.message || 'Login failed. Please try again.');
  }
};

export const register = async (userData) => {
  try {
    const response = await axios.post(`${API_URL}/Register`, userData);
 
    if (response.data.token && response.data.refreshToken && response.data.newUser) {
      Cookies.set('accessToken', response.data.token, cookieOptions);
      Cookies.set('refreshToken', response.data.refreshToken, cookieOptions);
      Cookies.set('user', JSON.stringify(response.data.newUser), cookieOptions);
    }

    return response.data;
  } catch (error) {
    console.error('Registration failed:', error.response?.data || error.message);
    throw new Error(error.response?.data?.message || 'Registration failed. Please try again.');
  }
};

export const logout = async () => {
  try {
    const token = getCurrentToken();

    await axios.post(
      `${API_URL}/Logout`, {},
      {
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      }
    );

    Cookies.remove('accessToken');
    Cookies.remove('refreshToken');
    Cookies.remove('user');
  } catch (error) {
    console.error('Logout failed:', error.response?.data || error.message);
    throw new Error(error.response?.data?.message || 'Logout failed. Please try again.');
  }
};

export const updateUser = async (formData) => {
  try{
    const token = getCurrentToken();
    const response = await axios.put(`${API_URL}/Edit`, formData, {
        headers: {
          'Authorization': `Bearer ${token}`,
          "Content-Type": "multipart/form-data",
        },
      });
    Cookies.set('user', JSON.stringify(response.data), cookieOptions)
  }catch(error){
    console.error(error);
  }
}

export const getCurrentToken = () => {
  const token = Cookies.get('accessToken');
  if (!token) {
    console.warn('Access token is not available.');
  }
  return token;
};

export const getCurrentUser = () => {
  const user = Cookies.get('user');
  return user ? JSON.parse(user) : null;
};

export const isAdmin = () => {
  const userCookie = Cookies.get('user');
  if (!userCookie) return false;

  try {
    const user = JSON.parse(userCookie);
    return user.role === 'admin';
  } catch (error) {
    console.error("Error:", error);
    return false;
  }
};

export const getCurrentUsername = () => {
  const userCookie = Cookies.get('user');

    if (!userCookie) {
        console.error("Cookie was not found.");
        return null;
    }

    try {
        const userData = JSON.parse(decodeURIComponent(userCookie));
        return userData.username;
    } catch (error) {
        console.error("Error:", error);
        return null;
    }
};

export const getCurrentUserId = () => {
  const userCookie = Cookies.get('user');

    if (!userCookie) {
        return null;
    }

    try {
        const userData = JSON.parse(decodeURIComponent(userCookie));
        return userData.id;
    } catch (error) {
        console.error("Error:", error);
        return null;
    }
};

export const refreshToken = async () => {
  try {
    const refreshToken = Cookies.get('refreshToken');
    const token = Cookies.get('accessToken');

    if (!refreshToken || !token) {
      throw new Error('No tokens available for refreshing.');
    }

    const response = await axios.post(`${API_URL}/refresh-token`, {
      token: token,
      refreshToken: refreshToken,
    });

    if (response.data.token && response.data.refreshToken) {
      Cookies.set('accessToken', response.data.token, cookieOptions);
      Cookies.set('refreshToken', response.data.refreshToken, cookieOptions);
    }

    return response.data;
  } catch (error) {
    console.error('Token refresh failed:', error.response?.data || error.message);
    throw new Error(error.response?.data?.message || 'Token refresh failed. Please login again.');
  }
};
