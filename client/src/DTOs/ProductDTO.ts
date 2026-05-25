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

export const maptToProductDto=(rawProduct : any): ProductDto=>{
    return{
        id: rawProduct.id ||-1,
        title: rawProduct.title || "Brak nazwy",
        description:rawProduct.description || '',
        imageUrl: rawProduct.imageUrl || null,
        categoryId: rawProduct.categoryId || -1,
        creationDate: rawProduct.creationDate ? new Date(rawProduct.creationDate) : null
        ,creatorUserId:rawProduct.creatorUserId || -1,
        comments: mapToCommentListDto(rawProduct.comments),
    productCategoryIdList: Array.isArray(rawProduct.productCategoryIdList) ? rawProduct.productCategoryIdList : []
  };  
}

export const mapToProductListDto=(rawInput:any):ProductDto[]=>{
    if (!Array.isArray(rawInput)){
        return [];
    };

    return rawInput.map((product)=>maptToProductDto(product))

  }