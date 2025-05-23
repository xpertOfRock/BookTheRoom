import axios from "axios";
import { getCurrentToken } from "./auth";

const API_URL = "https://localhost:6061/api/comment";

export const postComment = async ({description, propertyId, propertyType, userScore }) => {
  try {
    const token = getCurrentToken();

    const response = await axios.post(
      `${API_URL}`,
      { description, propertyId, propertyType, userScore },
      {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      }
    );
    return response.data;
  } catch (error) {
    console.error(
      "Error adding a new comment to the current hotel:",
      error.response ? error.response.data : error.message
    );
  }
};

export const getUserComments = async ( filter ) => {
  const token = getCurrentToken();

  try {
    const params = {
      search: filter?.search,
      sortItem: filter?.sortItem,
      sortOrder: filter?.sortOrder,
      itemsCount: filter?.itemsCount
    };

    const result = await axios.get(`${API_URL}/user-comments`, { params,
        headers: {
            'Authorization': `Bearer ${token}`,
          },
       },
    );
    console.log(result.data.apartments);
    return result.data.apartments;
  } catch (e) {
    console.error(e);
  }
}