import axios from 'axios';
import {  ProductDto,ProductListDto,mapToProductListDto,mapToProductDto } from '@/DTOs/ProductDTO';

const BASE_URL='https://localhost:7154/api/products';

export const productService={

    async getProductList(): Promise<ProductListDto[]>  {
  try {
    const response = await axios.get(BASE_URL)
    const rawProducts = response.data.products;

    return mapToProductListDto(rawProducts)
   
  } catch (error) {
    console.error("Błąd w serwisie podczas pobierania produktów:", error);
      throw error;  }
};

  async getSingleProduct( id: string): Promise<ProductDto>  {
  try {
    const response = await axios.get(BASE_URL+'/${id}')
    const rawProduct = response.data.product;

    return mapToProductDto(rawProduct)
   
  } catch (error) {
    console.error("Błąd w serwisie podczas pobierania produktów:", error);
      throw error;  }
}
}

