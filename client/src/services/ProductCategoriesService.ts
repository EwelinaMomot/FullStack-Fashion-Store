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
    },

    async deleteCategory(categoryId:number){
        try{
            const request = await axios.delete(`${BASE_URL}/${categoryId}`)
            return request.data
        }catch(e){
            console.log("Błąd podczas usuwania kategorii", e)
            throw(e)
        }
    },

    async editCategory(categoryId:number, category:NewCategoryDTO){
        try{
           const request = await axios.put(`${BASE_URL}/${categoryId}`, category)
            return request.data
        }catch(e){
            console.log("Błąd podczas edytowania kategorii", e)
            throw(e)
        }
    }
}
