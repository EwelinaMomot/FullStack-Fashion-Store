
import { CommentDto } from "@/DTOs/CommentDto";

export const mapToCommentDto =(rawInput :any):CommentDto=>{
    return{
        id:rawInput.id || null,
        description: rawInput.description ||'',
        creationDate: rawInput.creationDate ? new Date(rawInput.creationDate):null,
    }
}

export const mapToCommentListDto =(rawInput :any):CommentDto[]=>{
    if (!Array.isArray(rawInput)) {
    return [];
  }
  return rawInput.map((comment:any)=> mapToCommentDto(comment));
}