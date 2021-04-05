using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pet.Tracker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    [AuthorizeToken]
    public class CentersController : Controller
    {

        #region Protected Members


        /// <summary>
        /// The manager for handling user creation, deletion, searching, roles etc...
        /// </summary>
        protected UserManager<ApplicationUser> mUserManager;

        /// <summary>
        /// The Repository for handling user creation, deletion, searching etc...
        /// </summary>
        protected IVetCentersRepo Repo { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">The injected context</param>
        /// <param name="userManager">The Identity user manager</param>
        public CentersController(
            UserManager<ApplicationUser> userManager,
            IVetCentersRepo repo)
        {
         
            mUserManager = userManager;
            Repo = repo;

        }

        #endregion

        /// <summary>
        /// Tries to add a center for on the server
        /// </summary>
        /// <param name="model">The vet centers details</param>
        /// <returns>Returns the result of the register request</returns>
        [AllowAnonymous]
        [Route(CentersApiRoutes.Register)]
        public async Task<ApiResponse> AddCentersAsync([FromBody]VetCentersApiModel  model)
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

            var vetCenter = new VetCentersDataModel
            {
                Title = model.Title,
                Address = model.Address,
                Area = model.Area,
                State = model.State,
            };

            // Create the desired center from the given details
            //Try and add the center
            var result = await Repo.Add(vetCenter, true);
             
            // failed then return an error
            if(result == 0)
                // Return failed Response
                return errorResponse;

            //if it got here then it was successful
            return new ApiResponse();
           
           

        }


        /// <summary>
        /// Tries to update a center on the server
        /// </summary>
        /// <param name="model">The vet center details</param>
        /// <returns>Returns the result of a successful request</returns>
        [Route(CentersApiRoutes.UpdateCenter)]
        public async Task<ApiResponse> UpdateVetCenterAsync([FromBody]VetCentersApiModel model)
        {
            // TODO: Localize all strings
            // The message when we fail to login
            var invalidErrorMessage = "Please provide all required details to register the Pet";


            // The error Response for a failed registration
            var errorResponse = new ApiResponse<VetCentersApiModel>
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

            var vetCenter = await Repo.Find(model.Id);

            #region Update center


    

            // If we have a title...
            if (model.Title != null & !string.IsNullOrEmpty(model.Title))
                // Update the centers details
                vetCenter.Title = model.Title;              

            // If we have a state...
            if (model.State != null & !string.IsNullOrEmpty(model.State))
                // Update the centers details
                vetCenter.State = model.State;

            // If we have a area...
            if (model.Area != null & !string.IsNullOrEmpty(model.Area))
                // Update the center details
                vetCenter.Area = model.Area;

            // If we have a address...
            if (model.Address != null & !string.IsNullOrEmpty(model.Address))
                // Update the center details
                vetCenter.Address = model.Address;     
      
           

            #endregion

            #region Response

            //Try and add the center
            var result =  Repo.Update(vetCenter);

            // failed then return an error
            if (result == 0)
                // Return failed Response
                return errorResponse;

            //if it got here then it was successful
            return new ApiResponse();

            #endregion


        }

        /// <summary>
        /// Tries to delete a vet center on the server
        /// </summary>
        /// <param name="model">The vet center details</param>
        /// <returns>Returns the result of a successful request</returns>
        [Route(CentersApiRoutes.DeleteCenter)]
        public async Task<ApiResponse> DeleteVetCenterAsync([FromBody] VetCentersApiModel model)
        {
            // TODO: Localize all strings
            // The message when we fail to login
            var invalidErrorMessage = "Failed to Delete";


            // The error Response for a failed registration
            var errorResponse = new ApiResponse<VetCentersApiModel>
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

            //get current time stamp from the pets information
            var vet = await Repo.GetSingleWithDetails(model.Id);
           
            //Try and add the pet
            var result = Repo.Delete(vet.Id, vet.TimeStamp, true);

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
        [Route(CentersApiRoutes.SearchCenters)]
        public async Task<ApiResponse<VetCentersResultsApiModel>> SearchCentersAsync([FromBody]VetCentersApiModel model)
        {
            #region Get User

            // Get the current user
            var user = await mUserManager.GetUserAsync(HttpContext.User);

            // If we have no user...
            if (user == null)
                return new ApiResponse<VetCentersResultsApiModel>
                {
                    // TODO: Localization
                    ErrorMessage = "User not found"
                };

            #endregion

            #region Check Valid Search Credentials

            // Check if the user provided both a first and last name
            var titleOrArea = string.IsNullOrEmpty(model?.Area) || string.IsNullOrEmpty(model?.Title);

           
            // If we don't have enough details for a search...
            if (titleOrArea)
                // Return error
                return new ApiResponse<VetCentersResultsApiModel>
                {
                    // TODO: Localization
                    ErrorMessage = "Please provide a name, or the breed"
                };

            #endregion

            #region Find Pets

            // Create a found user variable
            var center = default(IEnumerable<VetCentersDataModel>);

            // If we have a title...
            if (!string.IsNullOrEmpty(model.Title))
                // Find the user by breed
                center = await Repo.SearchAsync(model.Title, null);

            // If we have an name...
            if (center == null && !string.IsNullOrEmpty(model.Area))
                // Find the user by name
                center = await Repo.SearchAsync(null, model.Area);

            //if nothing exist in the database return an error message
            if(center == null)
            {
                return new ApiResponse<VetCentersResultsApiModel>
                {
                    ErrorMessage = "No pets found"
                };
            }
      
            // If we found a pet...
            if (center.Any())
            {    
                // Return that pets details
                return new ApiResponse<VetCentersResultsApiModel>
                {
                    Response = new VetCentersResultsApiModel
                        {
                         
                           new VetCenterResultApiModel
                           {
                               Title =  center.FirstOrDefault().Title,
                               Area = center.FirstOrDefault().Area,
                               Id = center.FirstOrDefault().Id
                           }

                        }
                };
            }

            // Create a new list of results
            var results = new VetCentersResultsApiModel();

            // If we have a first and last name...
            if (!titleOrArea)
            {
                // Search for Pets...
                var vetCenters = Repo.GetAll().Take(100).ToList().OrderBy(x => x.Title);

                // If we found any Pets...
                if (center.Any())
                {
                    // Add each Pets details
                    results.AddRange(center.Select(u => new VetCenterResultApiModel
                    {
                      Id = u.Id,
                      Title = u.Title,
                      Area = u.Area
                         
                    })); 
                }
            }

            // Return the results
            return new ApiResponse<VetCentersResultsApiModel>
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
        [Route(CentersApiRoutes.GetAllCenters)]
        public async Task<ApiResponse<VetCentersResultsApiModel>> GetPetsAsync()
        {
            #region Get User

            // Get the current user
            var user = await mUserManager.GetUserAsync(HttpContext.User);

            // If we have no user...
            if (user == null)
                return new ApiResponse<VetCentersResultsApiModel>
                {
                    // TODO: Localization
                    ErrorMessage = "User not found"
                };

            #endregion

            #region Get Pets

            // Get the list of pets 
            var vetCenters = Repo.GetAll().Take(100).ToList();

            // Create a new list of results
            var results = new VetCentersResultsApiModel();
           
         
            // Add each Pets details
            results.AddRange(vetCenters.Select(u => new VetCenterResultApiModel
            {
            
                      
                         Area = u.Area,
                         Address = u.Address,
                         State = u.State,
                         Title = u.Title,
                         TimeStamp = u.TimeStamp,
                         Id = u.Id
                        
                  

            }));

            // If we found a pet...

            // Return that pets details
            return new ApiResponse<VetCentersResultsApiModel>
            {
                Response = results


            };
         

            #endregion
        }



    }
}
