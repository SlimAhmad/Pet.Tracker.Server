using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pet.Tracker.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    [AuthorizeToken]
    public class TransferController : Controller
    {
        #region Protected Members


        /// <summary>
        /// The manager for handling user creation, deletion, searching, roles etc...
        /// </summary>
        protected UserManager<ApplicationUser> mUserManager;

        /// <summary>
        /// The Repository for handling user creation, deletion, searching etc...
        /// </summary>
        protected ITransfer Repo { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">The injected context</param>
        /// <param name="userManager">The Identity user manager</param>
        public TransferController(
            UserManager<ApplicationUser> userManager,
            ITransfer repo)
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
        [Route(TransferApiRoutes.Register)]
        public async Task<ApiResponse> TransferPetAsync([FromBody] TransferPetApiModel model)
        {
            // TODO: Localize all strings
            // The message when we fail to login
            var invalidErrorMessage = "Please provide all required details to register the Pet";


            // The error Response for a failed registration
            var errorResponse = new ApiResponse<TransferPetApiModel>
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


            // generate a  4 digits token
            var token = GenerateToken();

            // Send a confirmation token to the user via sms
            //await PetTrackerEmailSender.SendSMSUserTokenAsync(user.FirstName, user.LastName, user.PhoneNumber, token);




            var transferPet = new TransferDataModel
            {

                TranferPhone = model.TransferPhone,
                TransfererToken = token,
                Username = user.UserName.ToString(),
                TransferEmail = model.TransferEmail,
                PetId = model.PetId,
                ClientId = user.Id.ToString(),
                Transfer = model.Transfer
            };

            // Create the desired center from the given details
            //Try and add the center
            var result = await Repo.Add(transferPet, true);

            // failed then return an error
            if (result == 0)
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
        public async Task<ApiResponse> AcceptOrCancelTransferAsync([FromBody] UpdateTransferApiModel model)
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

            var transfer = await Repo.Find(model.Id);

            #region Update Pet Profile

            // Create an instance of pets information
            var transferPet = new TransferDataModel();

            // If we have an Age...
            if (model.ConfirmToken == false & transfer.ReceiverToken != model.ReceiverToken)
            {
                transferPet.ConfirmToken = model.ConfirmToken;
                return new ApiResponse
                {
                    Response = "You canceled the transfer"
                };
            }
            else
            {
                // Update the profile details
                transfer.ConfirmToken = model.ConfirmToken;
            }


            #endregion

            #region Response

            //Try and add the pet
            var result = Repo.Update(transferPet);

            // failed then return an error
            if (result == 0)
                // Return failed Response
                return errorResponse;

            //if it got here then it was successful
            return new ApiResponse();

            #endregion


        }


        /// <summary>
        /// Tries to register for a new account on the server
        /// </summary>
        /// <param name="model">The pets details</param>
        /// <returns>Returns the result of a successful request</returns>
        [Route(PetApiRoutes.UpdatePetProfile)]
        public async Task<ApiResponse> SendVerificationTokenAsync([FromBody] UpdateTransferApiModel model)
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

            var transfer = await Repo.Find(model.Id);

            #region Update Pet Profile

            var token = GenerateToken();

            // Create an instance of pets information
            var transferPet = new TransferDataModel();

            transfer.ReceiverToken = token;

            await PetTrackerEmailSender.SendSMSUserTokenAsync(user.FirstName, user.LastName, user.PhoneNumber, token);

            #endregion

            #region Response

            //Try and add the pet
            var result = Repo.Update(transferPet);

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
        [Route(TransferApiRoutes.DeletePetTransfer)]
        public async Task<ApiResponse> DeleteTransferAsync([FromBody] TransferPetApiModel model)
        {
            // TODO: Localize all strings
            // The message when we fail to login
            var invalidErrorMessage = "Failed to Delete";


            // The error Response for a failed registration
            var errorResponse = new ApiResponse<TransferPetApiModel>
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
            var tranfer = await Repo.Find(model.Id);


            //Try and add the pet
            var result = Repo.Delete(tranfer.Id, tranfer.TimeStamp, true);

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
        [Route(TransferApiRoutes.GetAllTransfers)]
        public async Task<ApiResponse<TransferPetsResultsApiModel>> GetTransfersAsync()
        {
            #region Get User

            // Get the current user
            var user = await mUserManager.GetUserAsync(HttpContext.User);

            // If we have no user...
            if (user == null)
                return new ApiResponse<TransferPetsResultsApiModel>
                {
                    // TODO: Localization
                    ErrorMessage = "User not found"
                };

            #endregion

            #region Get Pets

            // Get the list of pets 
            var transfers = Repo.GetAllPetsTransferAsync().Result.Take(100).ToList();

            // Create a new list of results
            var results = new TransferPetsResultsApiModel();


            // Add each Pets details
            results.AddRange(transfers.Select(u => new TransferPetResultApiModel
            {


                TransferPhone = u.TransferPhone,
                TransfererToken = u.TransfererToken,
                TransferEmail = u.TransferEmail,
                Username = u.Username,
                Email = u.Email,
                Id = u.Id,
                PetId = u.PetId,
                Height = u.Height,
                Description = u.Description,
                Gender = u.Gender,
                Name = u.Name,
                Breed = u.Breed,
                Neutered = u.Neutered,
                Age = u.Age,
                PetOwnerName = u.PetOwnerName



            }));

            // If we found a pet...

            // Return that pets details
            return new ApiResponse<TransferPetsResultsApiModel>
            {
                Response = results


            };


            #endregion
        }

        #region Helper methods

        /// <summary>
        /// Generates a random token
        /// </summary>
        /// <returns></returns>
        private string GenerateToken()
        {
            var rand = new Random();
            return rand.Next(0000, 9999).ToString("D4");

        }
        #endregion


    }
}
