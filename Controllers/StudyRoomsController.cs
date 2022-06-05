﻿using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using asp_net_po_schedule_management_server.Dto;
using asp_net_po_schedule_management_server.Utils;
using asp_net_po_schedule_management_server.Entities;
using asp_net_po_schedule_management_server.Services;
using asp_net_po_schedule_management_server.Services.Helpers;
using asp_net_po_schedule_management_server.CustomDecorators;


namespace asp_net_po_schedule_management_server.Controllers
{
    /// <summary>
    /// Kontroler przechowujący akcje do zarządzania encją sal zajęciowych w systemie. Umożliwia stworzenie sali,
    /// pobranie wszystkich wyników oraz zaawanowaną filtrację wyników i paginację, pobieranie sal i dodatkowe filtrowanie
    /// (po nazwie) oraz standardowe metody usuwające zawartość z bazy danych.
    /// </summary>
    [ApiController]
    [Route("/api/v1/dotnet/[controller]")]
    [AuthorizeRoles(AvailableRoles.ADMINISTRATOR)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public sealed class StudyRoomsController : ControllerBase
    {
        private readonly IStudyRoomsService _service;
        private readonly ServiceHelper _helper;

        //--------------------------------------------------------------------------------------------------------------
        
        public StudyRoomsController(IStudyRoomsService service, ServiceHelper helper)
        {
            _service = service;
            _helper = helper;
        }

        //--------------------------------------------------------------------------------------------------------------
        
        [HttpPost(ApiEndpoints.ADD_STUDY_ROOM)]
        public async Task<ActionResult<StudyRoomResponseDto>> AddNewStudyRoom(StudyRoomRequestDto dto)
        {
            return StatusCode((int) HttpStatusCode.Created, await _service.CreateStudyRoom(dto));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        [HttpPut(ApiEndpoints.UPDATE_STUDY_ROOM)]
        public async Task<ActionResult<StudyRoomResponseDto>> UpdateStudyRoom(
            [FromBody] StudyRoomRequestDto dto,
            [FromQuery] long roomId)
        {
            return StatusCode((int) HttpStatusCode.OK, await _service.UpdateStudyRoom(dto, roomId));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        [HttpGet(ApiEndpoints.GET_ALL_STUDY_ROOMS)]
        public ActionResult<StudyRoomQueryResponseDto> GetStudyRooms([FromQuery] SearchQueryRequestDto searchSearchQuery)
        {
            return StatusCode((int) HttpStatusCode.OK, _service.GetAllStudyRooms(searchSearchQuery));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        [AllowAnonymous]
        [HttpGet(ApiEndpoints.GET_ALL_STUDY_ROOMS_SCHEDULE)]
        public async Task<ActionResult<List<NameWithDbIdElement>>> GetAllStudyRoomsScheduleBaseCath(
            [FromQuery] long deptId,
            [FromQuery] long cathId)
        {
            return StatusCode((int) HttpStatusCode.OK, await _service.GetAllStudyRoomsScheduleBaseCath(deptId, cathId));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        [AllowAnonymous]
        [HttpGet(ApiEndpoints.GET_ALL_STUDY_ROOMS_BASE_DEPT)]
        public async Task<ActionResult<List<NameWithDbIdElement>>> GetAllStudyRoomsScheduleBaseDeptName(
            [FromQuery] long deptId)
        {
            return StatusCode((int) HttpStatusCode.OK, await _service.GetAllStudyRoomsScheduleBaseDeptName(deptId));
        }

        //--------------------------------------------------------------------------------------------------------------
        
        [HttpGet(ApiEndpoints.GET_STUDY_ROOM_BASE_ID)]
        public async Task<ActionResult<StudyRoomEditResDto>> GetStudyRoomBaseDbId([FromQuery] long roomId)
        {
            return StatusCode((int) HttpStatusCode.OK, await _service.GetStudyRoomBaseDbId(roomId));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        [HttpDelete(ApiEndpoints.DELETE_MASSIVE)]
        public async Task<ActionResult> DeleteMassiveStudyRooms([FromBody] MassiveDeleteRequestDto deleteRooms)
        {
            await _service.DeleteMassiveStudyRooms(deleteRooms, _helper
                .ExtractedUserCredentialsFromHeader(HttpContext, this.Request));
            return StatusCode((int) HttpStatusCode.NoContent);
        }

        //--------------------------------------------------------------------------------------------------------------

        [HttpDelete(ApiEndpoints.DELETE_ALL)]
        public async Task<ActionResult> DeleteAllStudyRooms()
        {
            await _service.DeleteAllStudyRooms(_helper.ExtractedUserCredentialsFromHeader(HttpContext, this.Request));
            return StatusCode((int) HttpStatusCode.NoContent);
        }
    }
}