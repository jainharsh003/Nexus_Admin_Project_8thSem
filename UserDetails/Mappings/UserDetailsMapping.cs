    using UserDetails.DTOs;
    using UserDetails.Models;

    namespace UserDetails.Mappings
    {
        public static class UserDetailsMapping
        {
            public static UserDetailsEntity ToEntity(CreateUserDetailsDto dto, Guid userId)
            {
                return new UserDetailsEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,   
                    Name = dto.Name,
                    FatherName = dto.FatherName,
                    MotherName = dto.MotherName,
                    DOB = dto.DOB,
                    Age = dto.Age,
                    Gender = dto.Gender,
                    Field = dto.Field
                };
            }

            public static UserDetailsResponseDto ToDto(UserDetailsEntity entity)
            {
                return new UserDetailsResponseDto
                {
                    Id=entity.Id,
                    UserId = entity.UserId,
                    Name = entity.Name,
                    FatherName = entity.FatherName,
                    MotherName = entity.MotherName,
                    DOB = entity.DOB,
                    Age = entity.Age,
                    Gender = entity.Gender,
                    Field = entity.Field
                };
            }
        }
    }