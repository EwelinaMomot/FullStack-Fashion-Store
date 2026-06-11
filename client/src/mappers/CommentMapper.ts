
import { CommentDto } from "@/DTOs/CommentDto";
import { NewCommentDto } from "@/DTOs/NewCommentDto";

export const mapToCommentDto =(rawInput :any):CommentDto=>{
    return{
        id: rawInput.id || null,
        description: rawInput.description || '',
        creationDate: rawInput.creationDate ? new Date(rawInput.creationDate) : null,
        creatorUserId: rawInput.creatorUserId != null ? Number(rawInput.creatorUserId) : null,
    }
}

export const mapToCommentListDto =(rawInput :any):CommentDto[]=>{
    if (!Array.isArray(rawInput)) {
    return [];
  }
  return rawInput.map((comment:any)=> mapToCommentDto(comment));
}

export const mapToNewCommentDto = (rawInput: any): NewCommentDto => {
  return {
    productId: Number(rawInput.productId) || -1,
    description: rawInput.description || '',
    creatorUserId: Number(rawInput.creatorUserId) || -1,
  }
}
