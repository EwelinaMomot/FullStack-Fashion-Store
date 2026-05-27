import axios, { HttpStatusCode } from 'axios';
import {UserLoginDto} from '@/DTOs/UserLoginDto'

const BASE_URL='https://localhost:7154/api/auth';

export const AuthService={

    async login(credentials:UserLoginDto){
        try{
        const request = await axios.post(BASE_URL+'/login', credentials);
        return request.data

        }catch (e){
            console.error("Błąd podczas logowania",e)
            throw("Błąd podczas logowania")
        }

    },

     async register(credentials:UserLoginDto){
        try{
        const request = await axios.post(BASE_URL+'/register', credentials);
        return request.data

        }catch (e){
            console.error("Błąd podczas logowania",e)
            throw("Błąd podczas logowania")
        }

    }
}