# Hardware Protocol

현재 상태: R1 구조 조사 전

## 확인된 코드 이름

- `SerialManager`, `SerialPortNative`, `MainSerial`
- `PacketStructs`, `COMMTIMEOUTS`, `OVERLAPPED`
- `MotorDebug`, `MotorLv`, `MotorSync`
- `CardManager`

## 복구 원칙

- Gameplay은 장치 구현이 아닌 `IFishingInput` 같은 추상화에만 의존한다.
- Keyboard/Mouse/Inspector simulator를 먼저 만든다.
- Serial과 결제 장치는 연결 실패가 게임 루프를 중단하지 않도록 격리한다.
- 패킷 포맷, baud rate, timeout, 재연결 정책은 코드와 캡처 근거를 찾기 전 확정하지 않는다.

## 미확인

- COM 포트/baud rate
- IMU 데이터 단위와 endian
- Reel/Motor 명령 및 피드백 패킷
- 결제기 요청/응답 프로토콜
- watchdog와 재연결 조건

