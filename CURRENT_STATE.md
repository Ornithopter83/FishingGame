# Current State

업데이트: 2026-07-16

## 현재 단계

- Phase 0 기초 정보와 원본 구조 조사 완료
- Phase 1 기본 Scene 구조 완료
- Phase 2 첫 묶음 `Loading`, `LastScene`, `Title`은 시각 복원 판정을 취소하고 재작업 상태
- Unity 검증 버전: `2022.3.62f3`
- 이번 작업에서 빌드, 스테이징, 커밋, 푸시는 수행하지 않음

## 기본 정책

- `ExportedProject/`, `AuxiliaryFiles/`는 읽기 전용 원본 증거 영역이다.
- 채택한 자산과 코드는 `UnityProject/Assets`의 정상 제작 경로에 둔다.
- `_ReferenceTemp`는 임시 조사 복사본에만 사용하며 런타임 의존을 금지한다.
- 제작 자산과 클래스에 `Recovery` 접두어를 사용하지 않는다.
- 복원 상태는 `Original`, `Reconstructed`, `Estimated`, `Placeholder`로 문서화한다.
- 원본 근거 없는 Scene 화면 구성은 일체 금지한다.
- `Estimated`/`Placeholder`도 사용자 승인 없이 화면에 배치하지 않는다.

## 완료 내용

- 원본 구조 맵, DLL 분석, 자산 인벤토리 및 원본 SHA-256 manifest 작성
- 원본 파일 무결성 기준: 17,701개, 16,783,310,435 bytes
- 기본 제작 폴더와 `Bootstrap`, `Loading`, `LastScene`, `Title` Scene 구성
- PC 전용 Scene 흐름과 하드웨어 없는 입력 adapter 구성
- Loading/LastScene 기능 scaffold 구성. 시각 구성은 사용자 확인상 원본과 달라 재작업 대상
- Title을 사용자 제공 화면과 원본 YAML/GUID를 기준으로 실제 오브젝트로 재복원
  - 광안대교 Mesh, 수면, 요트 2척, 크루즈선
  - 중앙/좌상단 로고, 동적 결제 안내, 배터리, 장력, 버전 UI
  - 원본 선박 경로 좌표 기반 이동
  - Title BGM 및 `Title -> Loading -> LastScene` 흐름
- 원본 Title 유휴 루프 발견 및 문서화
  - 30초 대기
  - 랭킹 스크롤
  - 안내 이미지 2장
  - `fishing.webm` 또는 외부 `Intro/Intro.mp4` 재생
  - Title 복귀 후 반복
- 원본 Title 유휴 루프 구현
  - `GameSupport` 계층과 30개 랭킹 행, 안내 이미지 8개, `intro` VideoPlayer 복원
  - 원본 랭킹 좌표 `0 -> 765 -> 1545 -> 2120 -> 0` 및 대기/이동 시간 복원
  - `nRank1..30`, `sRank1..30`, `cRank1..30`, `dRank1..30` PlayerPrefs 계약 복원
  - 안내 이미지 2장 비반복 임의 선택, Loading BGM 전환, 영상 fade 및 Title 반복 복원
- Crest 근거의 우측 방향 수면 요동을 커스텀 수면 셰이더에 1차 반영 (`Estimated`)
- 전체 화면 참조 Sprite와 근거 없는 PC 메뉴/캐릭터 preview/button 제거
- Title 선택 자산을 정상 경로로 채택하고 원본 `.meta` GUID 보존

## Title 검증 상태

- Scene/compile 검증 통과: `UnityProject/Logs/TitleIdleLayoutFix.log`
- 실제 GPU 1920×1080 캡처: `UnityProject/Logs/TitleObjectRender.png`
- 30개 랭킹 GPU 캡처: `UnityProject/Logs/TitleRankRender.png`
- Play Mode 검증 통과: `UnityProject/Logs/TitleIdleLoopPlayModeFinal.log`
  - 결제 안내와 장력 Text 런타임 변경
  - 선박 Transform 이동
  - 단축 검증 모드로 랭킹 → 안내 이미지 2장 → 영상 → Title 복귀 1회 완료
  - `Title -> Loading -> LastScene -> Loading -> Title`
- 빌드는 수행하지 않았다.
- 위 검증은 현재 scaffold가 실행된다는 뜻이며 원본 시각·전체 동작 복원 완료를 뜻하지 않는다.
- Title 유휴 루프는 원본 코드/Scene 근거 범위에서 구현되었으나 실제 30초 전체 시간과 외부 영상 파일은 수동 장시간 검증이 남아 있다.

## 현재 한계

- Enviro 3 미확보: Procedural Sky 대체, 구름 누락 (`Estimated`)
- Crest 미확보: 커스텀 수면 대체 (`Estimated`)
- 원본 bridge shader 미확보: 원본 Albedo 기반 Standard 대체 (`Estimated`)
- SWS 미확보: 원본 경로 좌표 기반 `TitleShipMotion` 대체 (`Reconstructed`)
- Crest 원본의 우측 방향 파동을 1차 반영했지만 FFT, foam, 실제 Crest 수면과 동일하다고 볼 수 없음
- 랭킹 수량은 사용자 확인과 export 근거가 일치하는 30개로 확정
- 실제 Serial, 카드 단말기, IMU, 모터 장치 미검증
- Loading의 Spine animation과 LastScene의 실제 결과 데이터는 `Placeholder`

## 다음 작업

1. Title 수면의 우측 방향 파동을 작은 단계로 보정하고 사용자 비교를 받는다.
2. Unity에서 정상 30초 대기와 전체 랭킹 스크롤, 안내 이미지, 내장/외부 영상 재생을 수동 확인한다.
3. 결제 입력 발생 시 유휴 루프를 중단하고 결제 흐름을 우선하는 원본 조건을 추가 조사한다.
4. Loading과 LastScene은 원본 화면 근거가 확보된 요소만 다시 구성한다.
5. Task 06이 확정된 뒤 07번 범위를 논의한다.
