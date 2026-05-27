import axios from 'axios';
import {  ProductDto,ProductListDto,NewProductDto } from '@/DTOs/ProductDto';
import {mapToProductListDto,mapToProductDto} from '@/mappers/ProductMapper'

const BASE_URL='https://localhost:7154/api/products';

axios.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export const productService={

    async getProductList(): Promise<ProductListDto[]>  {
  try {
    const response = await axios.get(BASE_URL)
    const rawProducts = response.data.products;

    return mapToProductListDto(rawProducts)
   
  } catch (error) {
    console.error("Błąd w serwisie podczas pobierania produktów:", error);
      throw("Błąd w serwisie podczas pobierania produktów");  }
},

  async getSingleProduct( id: number): Promise<ProductDto>  {
  try {
    const response = await axios.get(BASE_URL+`/${id}`)
    const rawProduct = response.data;
    
    return mapToProductDto(rawProduct)
   
  } catch (error) {
    console.error("Błąd w serwisie podczas pobierania produktu:", error);
      throw("Błąd w serwisie podczas pobierania produktu");  }
},

async addProduct(newProduct:NewProductDto){
   try {
    const response = await axios.post(BASE_URL,newProduct,)
    return 
   
  } catch (error) {
    console.error("Błąd w serwisie podczas dodawania produktu:", error);
      throw("Błąd w serwisie podczas dodawania produktu");  }
}
}

