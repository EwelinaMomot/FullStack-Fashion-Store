import { NewCategoryDTO } from '@/DTOs/NewCategoryDTO';
import axios from 'axios';

const BASE_URL='https://localhost:7154/api/categories';

export const productCategoryService={
    async getAllCategories(){
        try{
            const request = await axios.get(BASE_URL)
            return request.data
        }catch(e){
            console.log("Błąd podczas pobierania listy kategorii", e)
            throw(e)
        }
    },

    async addCategory(category:NewCategoryDTO){
        try{
            const request = await axios.post(BASE_URL, category)
            return request.data
        }catch(e){
            console.log("Błąd podczas dodawania kategorii", e)
            throw(e)
        }
    }
}
