import { UserDto } from '@/DTOs/UserDto'

export const mapToUserDto = (rawUser: any): UserDto => ({
  id: rawUser.id ?? -1,
  username: rawUser.username ?? '',
  roleName: rawUser.roleName ?? rawUser.role ?? ''
})

export const mapToUserListDto = (rawInput: any): UserDto[] => {
  if (!Array.isArray(rawInput)) {
    return []
  }

  return rawInput.map(mapToUserDto)
}
