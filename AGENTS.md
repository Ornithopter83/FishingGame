# AGENTS.md

이 저장소는 기존 Unity 낚시 게임의 복구 프로젝트다.

## 작업 전 필독 순서

1. `AGENTS.md`
2. `CURRENT_STATE.md`
3. `PROJECT_SETUP_GUIDE.md`
4. `Docs/DLL_ANALYSIS.md`
5. `Docs/ASSET_INVENTORY.md`
6. 현재 `tasks/*.md`

## 절대 규칙

- `ExportedProject/`와 `AuxiliaryFiles/`는 원본 보존 영역이다. 이동, 이름 변경, 수정, 삭제하지 않는다.
- 원본 자료가 필요하면 조사 후 `UnityProject/Assets/OriginalImported/`로 원본 파일과 `.meta`를 함께 복사한다.
- `OriginalImported/` 안의 파일은 복사 후에도 직접 수정하지 않는다. 수정본은 `_Recovery/Recovered/`에 둔다.
- 근거 없는 동작과 수치를 원본 사실처럼 확정하지 않는다. `Reconstructed`, `Estimated`, `Placeholder` 중 하나로 표시한다.
- Missing Script는 삭제하지 말고 YAML script GUID, `.meta`, 코드/DLL 타입 순서로 조사한다.
- Unity 버전 변경 전에 `Assets`, `Packages`, `ProjectSettings`, 모든 `.meta`의 보존 상태를 확인한다.
- 요청 없이 `git commit`, `git pull`, `git push`를 실행하지 않는다.
- 요청 없이 Unity 빌드를 실행하지 않는다.

## 작업 방식

- 구현 전에 `tasks/00_TEMPLATE.md`를 복사해 작업 문서를 만든다.
- 한 작업은 가능한 한 코드 2~8개, Scene/Prefab 0~2개로 제한한다.
- 상태 전환은 추적 가능한 이유와 함께 기록한다.
- 작업 완료 시 해당 task의 결과와 `CURRENT_STATE.md`를 갱신한다.
- 원본과 무관한 리팩터링, 패키지 업그레이드, Addressables/ECS/DI 전환은 기능 복구 후 별도 작업으로 분리한다.

## 검증

- 정적 검증만 한 경우 Unity에서 확인한 것처럼 표현하지 않는다.
- 컴파일, EditMode/PlayMode 테스트, 수동 Unity 테스트 결과를 서로 분리해 기록한다.
- 하드웨어 및 결제 장치가 없어도 시뮬레이터로 핵심 루프가 중단되지 않아야 한다.

