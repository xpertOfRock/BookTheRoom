import axios from 'axios';

export const login = async (emailOrUsername, password) => {
    try {
        const response = await axios.post('https://localhost:5286/api/account/login', {
          emailOrUsername,
          password,
        });

        const token = response.data.token;

        localStorage.setItem('jwtToken', token);

        return response;
    } catch (error) {
        console.error('Error logging in:', error);
        throw error;
    }
};
