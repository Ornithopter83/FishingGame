# Hardware and PC Mode Boundary

현재 상태: R1 실행 모드 분기 확인

## 확인된 원본 구조

- `SerialManager.eGameMode`: `SERIAL`, `COMPUTER`
- `GameMode.txt`: `COM` 또는 `SERIAL`로 실행 장치 모드 선택
- `COMPUTER`에서는 Serial 연결 coroutine 대신 `Init()` 실행
- Loading Scene은 `SERIAL`에서만 slave 연결을 기다림
- `FishGameManager`: `Q/A` 키를 좌·우 버튼 입력으로 변환
- 일부 낚시 상태는 방향키를 PC 입력으로 사용
- `SerialManager`, `SerialPortNative`, `MainSerial`, `MotorDebug`, `MotorLv`, `MotorSync`, `CardManager`가 장치 계층을 구성

## 복원 원칙

- PC 모드를 기본 실행 모드로 둔다.
- Gameplay은 `IFishingInput`과 hardware output facade 같은 계약에 의존한다.
- Keyboard/Mouse/Inspector simulator를 먼저 구현한다.
- Serial과 결제 장치는 연결 실패가 게임 loop와 Scene 전환을 막지 않도록 격리한다.
- PC backend는 장치 출력 명령을 no-op 또는 로그로 처리한다.
- Serial backend는 실제 장치가 준비된 뒤 동일 계약으로 연결한다.
- 패킷 포맷, baud rate, timeout, 재연결 정책은 코드와 캡처 근거를 찾기 전 확정하지 않는다.
- 원본의 OS 종료 및 장치 강제 제어 코드는 안전 검토 전 실행하지 않는다.

## 후속 조사

- COM 포트와 baud rate
- IMU 데이터 단위와 endian
- Reel/Motor 명령 및 피드백 packet
- 결제기 요청/응답 protocol
- watchdog, timeout, reconnect 조건

