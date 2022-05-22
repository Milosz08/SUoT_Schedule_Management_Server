﻿using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using asp_net_po_schedule_management_server.Dto;
using asp_net_po_schedule_management_server.Utils;
using asp_net_po_schedule_management_server.Services;
using asp_net_po_schedule_management_server.Entities;
using asp_net_po_schedule_management_server.CustomDecorators;
using asp_net_po_schedule_management_server.Services.Helpers;


namespace asp_net_po_schedule_management_server.Controllers
{
    /// <summary>
    /// Kontroler przechowujący akcje do zarządzania encją grup w systemie. Umożliwia stworzenie grupy, pobranie
    /// wszystkich wyników oraz zaawanowaną filtrację wyników i paginację oraz standardowe metody usuwające zawartość
    /// z bazy danych.
    /// </summary>
    [ApiController]
    [Route("/api/v1/dotnet/[controller]")]
    [AuthorizeRoles(AvailableRoles.ADMINISTRATOR)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public sealed class StudyGroupController : ControllerBase
    {
        private readonly ServiceHelper _helper;
        private readonly IStudyGroupService _service;
        
        //--------------------------------------------------------------------------------------------------------------
        
        public StudyGroupController(IStudyGroupService service, ServiceHelper helper)
        {
            _service = service;
            _helper = helper;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        [HttpPost(ApiEndpoints.ADD_STUDY_GROUP)]
        public async Task<ActionResult<List<CreateStudyGroupResponseDto>>> AddNewStudyGroup(
            [FromBody] CreateStudyGroupRequestDto dto)
        {
            return StatusCode((int) HttpStatusCode.Created, await _service.CreateStudyGroup(dto));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        [HttpGet(ApiEndpoints.GET_ALL_STUDY_GROUPS_PAGINATION)]
        public ActionResult<StudyGroupQueryResponseDto> GetStudyRooms([FromQuery] SearchQueryRequestDto searchSearchQuery)
        {
            return StatusCode((int) HttpStatusCode.OK, _service.GetAllStudyGroups(searchSearchQuery));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        [HttpDelete(ApiEndpoints.DELETE_MASSIVE)]
        public async Task<ActionResult> DeleteMassiveGroups([FromBody] MassiveDeleteRequestDto deleteGroups)
        {
            await _service.DeleteMassiveStudyGroups(deleteGroups, _helper
                .ExtractedUserCredentialsFromHeader(HttpContext, this.Request));
            return StatusCode((int) HttpStatusCode.NoContent);
        }

        //--------------------------------------------------------------------------------------------------------------

        [HttpDelete(ApiEndpoints.DELETE_ALL)]
        public async Task<ActionResult> DeleteAllGroups()
        {
            await _service.DeleteAllStudyGroups(_helper.ExtractedUserCredentialsFromHeader(HttpContext, this.Request));
            return StatusCode((int) HttpStatusCode.NoContent);
        }
    }
}