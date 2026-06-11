
export interface CommentDto {
    id: number | null;
    description: string;
    creationDate: Date | null;
    creatorUserId?: number | null;
}

