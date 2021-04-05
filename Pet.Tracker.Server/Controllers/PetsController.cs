using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pet.Tracker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    [AuthorizeToken]
    public class PetsController : Controller
    {

        #region Protected Members


        /// <summary>
        /// The manager for handling user creation, deletion, searching, roles etc...
        /// </summary>
        protected UserManager<ApplicationUser> mUserManager;

        /// <summary>
        /// The Repository for handling user creation, deletion, searching etc...
        /// </summary>
        protected IPetsRepo Repo { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">The injected context</param>
        /// <param name="userManager">The Identity user manager</param>
        public PetsController(
            UserManager<ApplicationUser> userManager, IPetsRepo repo)
        {
         
            mUserManager = userManager;
            Repo = repo;

        }

        #endregion

        /// <summary>
        /// Tries to register a pet for a new account on the server
        /// </summary>
        /// <param name="registerCredentials">The registration details</param>
        /// <returns>Returns the result of the register request</returns>
        [AllowAnonymous]
        [Route(PetApiRoutes.Register)]
        public async Task<ApiResponse> RegisterPetAsync([FromBody]PetsProfileApiModel registerCredentials)
        {
            // TODO: Localize all strings
            // The message when we fail to login
            var invalidErrorMessage = "Please provide all required details to register the Pet";


            // The error Response for a failed registration
            var errorResponse = new ApiResponse<PetsProfileApiModel>
            {
                // Set error message
                ErrorMessage = invalidErrorMessage
            };

            if (registerCredentials == null)
                // Return failed Response
                return errorResponse;

            // Make sure we have a Pet name
            if (string.IsNullOrWhiteSpace(registerCredentials.Name))
                // Return error message to user
                return errorResponse;
           
            //Gets the user claims
            var user = await mUserManager.GetUserAsync(HttpContext.User);

            var pet = new Pets
            {
                Name = registerCredentials.Name,
                Neutered = registerCredentials.Neutered,
                Height = registerCredentials.Height,
                Breed = registerCredentials.Breed,
                Age = registerCredentials.Age,
                Description = registerCredentials.Description,
                ClientId = user.Id.ToString(),
                Gender = registerCredentials.Gender,
                Weight = registerCredentials.Weight,
            };

            // Create the desired user from the given details
            //Try and add the pet
            var result = await Repo.Add(pet, true);
             
            // failed then return an error
            if(result == 0)
                // Return failed Response
                return errorResponse;

            //if it got here then it was successful
            return new ApiResponse();
           
           

        }


        /// <summary>
        /// Tries to register for a new account on the server
        /// </summary>
        /// <param name="model">The pets details</param>
        /// <returns>Returns the result of a successful request</returns>
        [Route(PetApiRoutes.UpdatePetProfile)]
        public async Task<ApiResponse> UpdatePetProfileAsync([FromBody]UpdatePetsProfileApiModel model)
        {
            // TODO: Localize all strings
            // The message when we fail to login
            var invalidErrorMessage = "Please provide all required details to register the Pet";


            // The error Response for a failed registration
            var errorResponse = new ApiResponse<PetsProfileApiModel>
            {
                // Set error message
                ErrorMessage = invalidErrorMessage
            };

            if (model == null)
                // Return failed Response
                return errorResponse;

            #region Get User

            // Get the current user
            var user = await mUserManager.GetUserAsync(HttpContext.User);

            // If we have no user...
            if (user == null)
                return new ApiResponse
                {
                    // TODO: Localization
                    ErrorMessage = "User not found"
                };

            #endregion

            var pets = await Repo.Find(model.Id);

            #region Update Pet Profile


      

            // If we have a name...
            if (model.Name != null)
                // Update the profile details
                pets.Name = model.Name;              

            // If we have an Age...
            if (model.Breed != null)
                // Update the profile details
                pets.Breed = model.Breed;

            // If we have a description...
            if (model.Description != null)
                // Update the profile details
                pets.Description = model.Description;

            // If we have a Gender...
            if (model.Gender != null)
                // Update the profile details
                pets.Gender = model.Gender;     
      
            // Update the profile details
            pets.Neutered = model.Neutered;
            pets.Weight = model.Weight;
            pets.Age = model.Age;
            pets.Height = model.Height;
           

            #endregion

            #region Response

            //Try and add the pet
            var result =  Repo.Update(pets);

            // failed then return an error
            if (result == 0)
                // Return failed Response
                return errorResponse;

            //if it got here then it was successful
            return new ApiResponse();

            #endregion


        }

        /// <summary>
        /// Tries to delete a pets account on the server
        /// </summary>
        /// <param name="model">The pets details</param>
        /// <returns>Returns the result of a successful request</returns>
        [Route(PetApiRoutes.DeletePetProfile)]
        public async Task<ApiResponse> DeletePetProfileAsync([FromBody] PetsProfileApiModel model)
        {
            // TODO: Localize all strings
            // The message when we fail to login
            var invalidErrorMessage = "Failed to Delete";


            // The error Response for a failed registration
            var errorResponse = new ApiResponse<PetsProfileApiModel>
            {
                // Set error message
                ErrorMessage = invalidErrorMessage
            };

            if (model == null)
                // Return failed Response
                return errorResponse;

            #region Get User

            // Get the current user
            var user = await mUserManager.GetUserAsync(HttpContext.User);

            // If we have no user...
            if (user == null)
                return new ApiResponse
                {
                    // TODO: Localization
                    ErrorMessage = "User not found"
                };

            #endregion

            #region Response

            // the desired Pet from the given details
            var pets = new Pets
            {
                Id = model.Id,
                Name = model.Name,
                Neutered = model.Neutered,
                Height = model.Height,
                Breed = model.Breed,
                Age = model.Age,
                Description = model.Description,
                ClientId = user.Id.ToString(),
                Gender = model.Gender,
                Weight = model.Weight,

            };

            //get current time stamp from the pets information
            var pet = await Repo.GetSinglePetWithDetails(pets.ClientId);
           
            //Try and add the pet
            var result = Repo.Delete(pet.Id, pet.TimeStamp, true);

            // failed then return an error
            if (result == 0)
                // Return failed Response
                return errorResponse;

            //if it got here then it was successful
            return new ApiResponse();

            #endregion


        }


        /// <summary>
        /// Searches all pets for any users that match the search credentials
        /// </summary>
        /// <param name="model">The search credentials</param>
        /// <returns>
        ///     Returns a list of found contact details if successful, 
        ///     otherwise returns the error reasons for the failure
        /// </returns>
        [AllowAnonymous]
        [Route(PetApiRoutes.SearchPets)]
        public async Task<ApiResponse<SearchPetsResultsApiModel>> SearchUsersAsync([FromBody]SearchPetsApiModel model)
        {
            #region Get User

            // Get the current user
            var user = await mUserManager.GetUserAsync(HttpContext.User);

            // If we have no user...
            if (user == null)
                return new ApiResponse<SearchPetsResultsApiModel>
                {
                    // TODO: Localization
                    ErrorMessage = "User not found"
                };

            #endregion

            #region Check Valid Search Credentials

            // Check if the user provided both a first and last name
            var BreedOrNameMissing = string.IsNullOrEmpty(model?.Breed) || string.IsNullOrEmpty(model?.Name);

           
            // If we don't have enough details for a search...
            if (BreedOrNameMissing)
                // Return error
                return new ApiResponse<SearchPetsResultsApiModel>
                {
                    // TODO: Localization
                    ErrorMessage = "Please provide a name, or the breed"
                };

            #endregion

            #region Find Pets

            // Create a found user variable
            var foundPet = default(IEnumerable<Pets>);

            // If we have a breed...
            if (!string.IsNullOrEmpty(model.Breed))
                // Find the user by breed
                foundPet = await Repo.SearchAsync(model.Breed);

            // If we have an name...
            if (foundPet == null && !string.IsNullOrEmpty(model.Name))
                // Find the user by name
                foundPet = await Repo.SearchAsync(model.Name);

            //if nothing exist in the database return an error message
            if(foundPet == null)
            {
                return new ApiResponse<SearchPetsResultsApiModel>
                {
                    ErrorMessage = "No pets found"
                };
            }
      
            // If we found a pet...
            if (foundPet.Any())
            {    
                // Return that pets details
                return new ApiResponse<SearchPetsResultsApiModel>
                {
                    Response = new SearchPetsResultsApiModel
                        {
                         
                           new SearchPetResultApiModel
                           {
                               Breed =  foundPet.FirstOrDefault().Breed,
                               Name = foundPet.FirstOrDefault().Name,
                               Id = foundPet.FirstOrDefault().Id
                           }

                        }
                };
            }

            // Create a new list of results
            var results = new SearchPetsResultsApiModel();

            // If we have a first and last name...
            if (!BreedOrNameMissing)
            {
                // Search for Pets...
                var foundPets = Repo.GetAll().Take(100).ToList().OrderBy(x => x.Name);

                // If we found any Pets...
                if (foundPets.Any())
                {
                    // Add each Pets details
                    results.AddRange(foundPets.Select(u => new SearchPetResultApiModel
                    {
                      Id = u.Id,
                      Name = u.Name,
                      Breed = u.Breed
                         
                    })); 
                }
            }

            // Return the results
            return new ApiResponse<SearchPetsResultsApiModel>
            {
                Response = results
            };

            #endregion
        }


        /// <summary>
        /// Searches all pets for any users that match the search credentials
        /// </summary>
        /// <param name="model">The search credentials</param>
        /// <returns>
        ///     Returns a list of found contact details if successful, 
        ///     otherwise returns the error reasons for the failure
        /// </returns>
        [AllowAnonymous]
        [Route(PetApiRoutes.GetPetProfile)]
        public async Task<ApiResponse<PetResultApiModel>> GetPetAsync()
        {
            #region Get User

            // Get the current user
            var user = await mUserManager.GetUserAsync(HttpContext.User);

            // If we have no user...
            if (user == null)
                return new ApiResponse<PetResultApiModel>
                {
                    // TODO: Localization
                    ErrorMessage = "User not found"
                };

            #endregion

            #region Get Pets

            // Get the list of pets 
            var pet = await Repo.GetSinglePetWithDetails(user.Id.ToString());


            // Create a new pet 
            // Add each Pets details
            var result = new PetResultApiModel
            {

                Age = pet.Age,
                Name = pet.Name,
                Id = pet.Id,
                Breed = pet.Breed,
                Gender = pet.Gender,
                Description = pet.Description,
                Height = pet.Height,
                Weight = pet.Weight,
                Neutered = pet.Neutered,
                ClientId = pet.ClientId,
                TimeStamp = pet.TimeStamp



            };

            // If we found a pet...

            // Return that pets details
            return new ApiResponse<PetResultApiModel>
            {
                Response = result


            };


            #endregion
        }


        /// <summary>
        /// Searches all pets for any users that match the search credentials
        /// </summary>
        /// <param name="model">The search credentials</param>
        /// <returns>
        ///     Returns a list of found contact details if successful, 
        ///     otherwise returns the error reasons for the failure
        /// </returns>
        [AllowAnonymous]
        [Route(PetApiRoutes.GetAllPets)]
        public async Task<ApiResponse<PetsResultsApiModel>> GetPetsAsync()
        {
            #region Get User

            // Get the current user
            var user = await mUserManager.GetUserAsync(HttpContext.User);

            // If we have no user...
            if (user == null)
                return new ApiResponse<PetsResultsApiModel>
                {
                    // TODO: Localization
                    ErrorMessage = "User not found"
                };

            #endregion

            #region Get Pets

            // Get the list of pets 
            var pets = Repo.GetAll().Take(100).ToList();

            // Create a new list of results
            var results = new PetsResultsApiModel();
           
         
            // Add each Pets details
            results.AddRange(pets.Select(u => new PetResultApiModel
            {
            
                        Age = u.Age,
                        Name = u.Name,
                        Id = u.Id,
                        Breed = u.Breed,
                        Gender = u.Gender,
                        Description = u.Description,
                        Height = u.Height,
                        Weight = u.Weight,
                        Neutered = u.Neutered ,
                        ClientId = u.ClientId,
                        TimeStamp = u.TimeStamp
                        
                  

            }));

            // If we found a pet...

            // Return that pets details
            return new ApiResponse<PetsResultsApiModel>
            {
                Response = results


            };
         

            #endregion
        }



    }
}
