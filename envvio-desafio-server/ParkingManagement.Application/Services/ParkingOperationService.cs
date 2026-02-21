using AutoMapper;
using ParkingManagement.Application.DTOs;
using ParkingManagement.Application.Interfaces;
using ParkingManagement.Domain.Entities;
using ParkingManagement.Domain.Exceptions;
using ParkingManagement.Domain.Interfaces;

namespace ParkingManagement.Application.Services;

public class ParkingOperationService : IParkingOperationService
{
    private readonly IParkingSessionRepository _sessionRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IPricingService _pricingService;
    private readonly IMapper _mapper;

    public ParkingOperationService(
        IParkingSessionRepository sessionRepository,
        IVehicleRepository vehicleRepository,
        IPricingService pricingService,
        IMapper mapper)
    {
        _sessionRepository = sessionRepository;
        _vehicleRepository = vehicleRepository;
        _pricingService = pricingService;
        _mapper = mapper;
    }

    public async Task<ParkingSessionDto> RegisterEntryAsync(RegisterEntryDto dto)
    {
        var vehicle = await _vehicleRepository.GetByPlateAsync(dto.Plate)
            ?? throw new VehicleNotFoundException($"Vehicle with plate {dto.Plate} was not found. Please register the vehicle first.");

        if (await _sessionRepository.HasOpenSessionAsync(vehicle.Id))
            throw new VehicleAlreadyInParkingException(vehicle.Plate);

        var session = new ParkingSession(vehicle.Id);
        var created = await _sessionRepository.AddAsync(session);

        // Reload session with vehicle navigation property
        created = await _sessionRepository.GetByIdAsync(created.Id);
        
        return _mapper.Map<ParkingSessionDto>(created);
    }

    public async Task<ExitPreviewDto> PreviewExitByPlateAsync(string plate)
    {
        var vehicle = await _vehicleRepository.GetByPlateAsync(plate)
            ?? throw new VehicleNotFoundException($"Vehicle with plate {plate} was not found.");

        var session = await _sessionRepository.GetOpenSessionByVehicleIdAsync(vehicle.Id)
            ?? throw new ParkingSessionNotFoundException($"No open parking session found for vehicle {plate}.");

        if (!session.IsOpen)
            throw new InvalidParkingOperationException("Session is already closed");

        var duration = session.GetDuration();
        var amount = _pricingService.CalculateParkingFee(duration);

        return new ExitPreviewDto(
            session.Id,
            session.EntryTime,
            DateTime.UtcNow,
            duration,
            amount,
            _mapper.Map<VehicleDto>(session.Vehicle)
        );
    }

    public async Task<ParkingSessionDto> RegisterExitAsync(RegisterExitDto dto)
    {
        var vehicle = await _vehicleRepository.GetByPlateAsync(dto.Plate)
            ?? throw new VehicleNotFoundException($"Vehicle with plate {dto.Plate} was not found.");

        var session = await _sessionRepository.GetOpenSessionByVehicleIdAsync(vehicle.Id)
            ?? throw new ParkingSessionNotFoundException($"No open parking session found for vehicle {dto.Plate}.");

        if (!session.IsOpen)
            throw new InvalidParkingOperationException("Session is already closed");

        var duration = session.GetDuration();
        var amount = _pricingService.CalculateParkingFee(duration);

        session.RegisterExit(amount);
        await _sessionRepository.UpdateAsync(session);

        return _mapper.Map<ParkingSessionDto>(session);
    }

    public async Task<IEnumerable<ParkingSessionDto>> GetAllOpenSessionsAsync()
    {
        var sessions = await _sessionRepository.GetAllOpenSessionsAsync();
        return _mapper.Map<IEnumerable<ParkingSessionDto>>(sessions);
    }

    public async Task<ParkingSessionDto?> GetSessionByIdAsync(int id)
    {
        var session = await _sessionRepository.GetByIdAsync(id);
        return session != null ? _mapper.Map<ParkingSessionDto>(session) : null;
    }
}
