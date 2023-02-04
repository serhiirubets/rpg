using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character> {
            new Character(),
            new Character { Id = 1, Name = "Sam" }
        };
        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var serviceReponse = new ServiceResponse<List<GetCharacterDto>>();
            var newCharacter = _mapper.Map<Character>(character);
            newCharacter.Id = characters.Max(c => c.Id) + 1;
            characters.Add(newCharacter);
            serviceReponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceReponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceReponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {

                var character = characters.FirstOrDefault(c => c.Id == id);

                if (character is null)
                {
                    throw new Exception($"Character with ID '{id}'");
                }

                characters.Remove(character);

                serviceReponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception e)
            {
                serviceReponse.Success = false;
                serviceReponse.Message = e.Message;
            }

            return serviceReponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceReponse = new ServiceResponse<List<GetCharacterDto>>();
            serviceReponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList(); ;
            return serviceReponse;
        }


        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterByID(int id)
        {
            var character = characters.FirstOrDefault(c => c.Id == id);
            var serviceReponse = new ServiceResponse<GetCharacterDto>();

            serviceReponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceReponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceReponse = new ServiceResponse<GetCharacterDto>();

            try
            {

                var character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);

                if (character is null)
                {
                    throw new Exception($"Character with ID '{updatedCharacter.Id}'");
                }

                _mapper.Map(updatedCharacter, character);

                serviceReponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception e)
            {
                serviceReponse.Success = false;
                serviceReponse.Message = e.Message;
            }

            return serviceReponse;
        }
    }
}