import { CommentDto } from './CommentDto';

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

export interface NewProductDto {
    title: string;
    description: string;
    imageUrl: string | null;
    productCategoryIdList: Array<number>;
}


