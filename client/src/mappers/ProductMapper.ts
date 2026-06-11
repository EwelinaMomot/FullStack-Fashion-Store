import { ProductDetailDto } from "@/DTOs/ProductDto";
import { ProductListDto } from "@/DTOs/ProductDto";
import { mapToCommentListDto } from "./CommentMapper";

export const mapToProductDto=(rawProduct : any): ProductDetailDto=>{

    return{
        id: rawProduct.id ||-1,
        title: rawProduct.title || "Brak nazwy",
        description:rawProduct.description || '',
        imageUrl: rawProduct.imageUrl || null,
        categoryId: rawProduct.productCategoryId || -1,
        creationDate: rawProduct.creationDate ? new Date(rawProduct.creationDate) : null
        ,creatorUserId:rawProduct.creatorUserId || -1,
          
        comments: mapToCommentListDto(rawProduct.comments),
        productCategoryList: Array.isArray(rawProduct.categories) ? rawProduct.categories : [],
        productCategoryIdList: Array.isArray(rawProduct.categoriesIds) ? rawProduct.categoriesIds : [],
          }; 
           
}

export const mapToProductList=(rawProduct : any): ProductListDto=>{

    return{
        id: rawProduct.id ||-1,
        title: rawProduct.title || "Brak nazwy",
        description:rawProduct.description || '',
        imageUrl: rawProduct.imageUrl || null,
        categoryId: rawProduct.productCategoryId || -1,
        creationDate: rawProduct.creationDate ? new Date(rawProduct.creationDate) : null
        ,creatorUserId:rawProduct.creatorUserId || -1,
          };  
}

export const mapToProductListDto=(rawInput:any):ProductListDto[]=>{
    if (!Array.isArray(rawInput)){
        return [];
    };

    return rawInput.map((product)=>mapToProductList(product))

  }
