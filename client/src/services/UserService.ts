import { changeUserRoleDTO } from '@/DTOs/changeUserRoleDTO';
import { UserDto } from '@/DTOs/UserDto';
import { mapToUserListDto } from '@/mappers/UserMapper';
import axios from 'axios'

const BASE_URL='https://localhost:7154/api/users';


export const userService={

    async getUsersList():Promise<UserDto[]> {
        try {
            const request = await axios.get(BASE_URL + "/")
            const rawUsers = Array.isArray(request.data)
                ? request.data
                : Array.isArray(request.data?.users)
                    ? request.data.users
                    : []

            return mapToUserListDto(rawUsers)
        } catch(e) {
            console.log("Błąd podczas pobierania listy użytkowników:", e)
            throw("Błąd podczas pobierania listy użytkowników")
        }
    },

    async changeUserRole(payload:changeUserRoleDTO){
        try{
            const request = await axios.put(BASE_URL+`/${payload.userId}/role`,payload)
            return request.data
        }catch(e){
            console.log("Błąd przy zmianie roli użytkownika",e)
            throw("Błąd przy zmianie roli użytkownika")
        }
    }

    
}