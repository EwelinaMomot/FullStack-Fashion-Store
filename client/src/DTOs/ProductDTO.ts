import { CommentDto,mapToCommentListDto } from './CommentDto';

export interface ProductDto {
    id: number;
    title: string;
    description: string;
    imageUrl: string | null;
    categoryId: number;
    creationDate: Date | null;
    creatorUserId: number;
    comments: Array<CommentDto>;
    productCategoryIdList: Array<number>;
}

export interface ProductListDto {
    id: number;
    title: string;
    description: string;
    imageUrl: string | null;
    categoryId: number;
    creationDate: Date | null;
    creatorUserId: number;

}

export const mapToProductDto=(rawProduct : any): ProductDto=>{
    return{
        id: rawProduct.id ||-1,
        title: rawProduct.title || "Brak nazwy",
        description:rawProduct.description || '',
        imageUrl: rawProduct.imageUrl || null,
        categoryId: rawProduct.productCategoryId || -1,
        creationDate: rawProduct.creationDate ? new Date(rawProduct.creationDate) : null
        ,creatorUserId:rawProduct.creatorUserId || -1,
          
        comments: mapToCommentListDto(rawProduct.comments),
    productCategoryIdList: Array.isArray(rawProduct.productCategoryIdList) ? rawProduct.productCategoryIdList : []
  };  
}

export const mapToProductList=(rawProduct : any): ProductListDto=>{
    console.log("t",rawProduct)
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

