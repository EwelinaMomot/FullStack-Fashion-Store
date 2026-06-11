import axios from 'axios';
import { NewCommentDto } from '@/DTOs/NewCommentDto';

const BASE_URL = 'https://localhost:7154/api/comments';


export const commentService = {
  async getComments(productId: number, page = 1, pageSize = 10) {
    try {
      const url = `${BASE_URL}?productId=${productId}&page=${page}&pageSize=${pageSize}`;
      const response = await axios.get(url);
      return response.data;
    } catch (error) {
      console.error('Błąd podczas pobierania komentarzy:', error);
      throw(error);
    }
  },

  async addComment(comment: NewCommentDto) {
    try {
      const request = await axios.post(BASE_URL, comment);
      return request.data;
    } catch (error) {
      console.error('Błąd podczas dodawania komentarza:', error);
      throw(error);
    }
  },

  async deleteComment(commentId: number) {
    try {
      const request = await axios.delete(`${BASE_URL}/${commentId}`);
      return request.data;
    } catch (error) {
      console.error('Błąd podczas usuwania komentarza:', error);
      throw(error);
    }
  },

  async editComment(commentId: number, comment: { description: string }) {
    try {
      const request = await axios.put(`${BASE_URL}/${commentId}`, comment);
      return request.data;
    } catch (error) {
      console.error('Błąd podczas edycji komentarza:', error);
      throw(error);
    }
  }
};
