using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StateMachineStorage.Abstract.Repositories;
using StateMachineStorage.Abstract.Services;
using StateMachineStorage.Data;
using StateMachineStorage.Models;
using System;
using System.Collections.Generic;

namespace StateMachineStorage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StateMachineController : ControllerBase
    {
        private readonly IStateMachineService _stateMachineService;
        private readonly ILoggerService _loggerService;
        private readonly IStateMachineRepository _stateMachineRepository;

        public StateMachineController(IStateMachineService stateMachineService, ILoggerService loggerService, IStateMachineRepository stateMachineRepository)
        {
            _stateMachineService = stateMachineService;
            _stateMachineRepository = stateMachineRepository;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Check if state machine is valid against a schema
        /// </summary>
        /// <param name="stateMachine">State machine</param>
        /// <returns></returns>
        [HttpPost, Route("ValidateStateMachine"), DisableRequestSizeLimit]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult ValidateStateMachine([FromBody]StateMachineModel stateMachine)
        {
            try
            {
                var result = _stateMachineService.ValidateStateMachine(stateMachine);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggerService.Log("StateMachineController.ValidateStateMachine", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Add new state machine into a db
        /// </summary>
        /// <param name="stateMachine">State machine</param>
        /// <returns></returns>
        [HttpPost, Route("AddStateMachine"), DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult AddStateMachine([FromBody]StateMachineModel stateMachine)
        {
            try
            {
                _stateMachineRepository.AddStateMachine(stateMachine);

                return Ok();
            }
            catch (Exception ex)
            {
                _loggerService.Log("StateMachineController.AddStateMachine", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Add new state machine implementation into a db
        /// </summary>
        /// <param name="stateMachineImplementation">State machine implementation</param>
        /// <returns></returns>
        [HttpPost, Route("AddStateMachineImplementation"), DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult AddStateMachineImplementation([FromBody]StateImplementationModel stateMachineImplementation)
        {
            try
            {
                _stateMachineRepository.AddStateMachineImplementation(stateMachineImplementation);

                return Ok();
            }
            catch (Exception ex)
            {
                _loggerService.Log("StateMachineController.AddStateMachineImplementation", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Update state machine implementation
        /// </summary>
        /// <param name="stateMachineImplementation">State machine implementation</param>
        /// <returns></returns>
        [HttpPut, Route("UpdateStateMachineImplementation"), DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateStateMachineImplementation([FromBody] StateImplementationModel stateMachineImplementation)
        {
            try
            {
                _stateMachineRepository.UpdateStateMachineImplementation(stateMachineImplementation);

                return Ok();
            }
            catch (Exception ex)
            {
                _loggerService.Log("StateMachineController.UpdateStateMachineImplementation", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get state machine definitios from a DB
        /// </summary>
        /// <param name="agenda">Agenda to sort by</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<SMDefinition>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult GetStateMachines([FromQuery]string agenda)
        {
            try
            {
                var templates = _stateMachineRepository.GetStateMachines(agenda);

                return Ok(templates);
            }
            catch (Exception ex)
            {
                _loggerService.Log("StateMachineController.GetStateMachines", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get state machine definition by ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(SMDefinition), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult GetStateMachineById(long id)
        {
            try
            {
                var stateMachine = _stateMachineRepository.GetStateMachineById(id);

                return Ok(stateMachine);
            }
            catch (Exception ex)
            {
                _loggerService.Log("StateMachineController.GetStateMachineById", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Update state machine definition
        /// </summary>
        /// <param name="updatedStateMachine">Updated state machine definition</param>
        /// <returns></returns>
        [HttpPut, Route("UpdateStateMachine"), DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateStateMachine([FromBody]StateMachineModel updatedStateMachine)
        {
            try
            {
                _stateMachineRepository.UpdateStateMachine(updatedStateMachine);

                return Ok();
            }
            catch (Exception ex)
            {
                _loggerService.Log("StateMachineController.UpdateStateMachine", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
