import axios from 'axios';
import { ProductDetailDto, ProductListDto, ProductListPageDto, NewProductDto } from '@/DTOs/ProductDto';
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

    async getProductList(page = 1, pageSize = 3, categoryId?: number): Promise<ProductListPageDto> {
        try {
            let url = `${BASE_URL}?page=${page}&pageSize=${pageSize}`
            if (categoryId != null) {
                url += `&categoryId=${categoryId}`
            }

            const response = await axios.get(url)

            const rawProducts = response.data.Products ?? response.data.products ?? []
            return {
                products: mapToProductListDto(rawProducts),
                totalProductsNumber: response.data.TotalProductsNumber ?? 0,
                currentPage: response.data.CurrentPage ?? page,
                totalPages: response.data.totalPages ?? 1
            }
        } catch (error) {
            console.error("Błąd w serwisie podczas pobierania produktów:", error);
            throw("Błąd w serwisie podczas pobierania produktów");
        }
    },

  async getSingleProduct( id: number): Promise<ProductDetailDto>  {
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
    const response = await axios.post(BASE_URL,newProduct)
    return 
   
  } catch (error) {
    console.error("Błąd w serwisie podczas dodawania produktu:", error);
      throw("Błąd w serwisie podczas dodawania produktu");  }
  },


  async deleteProduct(productId:Number){
    try{
      const request = await axios.delete(BASE_URL+`/${productId}`)
      return
    }catch(e){
        console.error("Błąd podczas usuwania produktu:", e);
      throw("Błąd podczas usuwania produktu");  }
    
  },

  async editProduct(productId:Number,product:NewProductDto){
    try{
      const request = await axios.put(BASE_URL+`/${productId}`,product)
      console.log(request.data)
      return
    }catch(e){
        console.error("Błąd podczas edytowania produktu:", e);
      throw("Błąd podczas edytowania produktu");  }
    
  }


}

