# AGENTS.md

이 저장소는 조각난 Unity 낚시 게임을 새 Unity 프로젝트에서 복원하는 작업 공간이다.

## 작업 전 확인 순서

1. `AGENTS.md`
2. `CURRENT_STATE.md`
3. `PROJECT_SETUP_GUIDE.md`
4. `Docs/DLL_ANALYSIS.md`
5. `Docs/ASSET_INVENTORY.md`
6. 현재 `tasks/*.md`

## 안전 규칙

- `ExportedProject/`와 `AuxiliaryFiles/`는 읽기 전용 원본 증거 영역이다. 이동, 이름 변경, 수정, 삭제하지 않는다.
- 채택한 자산과 구현은 `UnityProject/Assets/Scripts`, `Scenes`, `Prefabs`, `Art`, `Audio` 등 정상 제작 경로에 둔다.
- 원본 `.meta`가 있고 유효하면 자산과 함께 가져와 GUID를 보존한다. 새 GUID를 만들기 전에 참조 관계를 조사한다.
- 조사나 변환을 위한 임시 복사본만 `UnityProject/Assets/_ReferenceTemp/`에 둘 수 있다. 런타임 자산이 이 폴더에 의존해서는 안 되며, 채택 후 정상 경로로 옮긴다.
- 출처와 복원 상태는 폴더 접두어가 아니라 task, 문서, 코드 주석 또는 자산 인벤토리에 `Original`, `Reconstructed`, `Estimated`, `Placeholder`로 기록한다.
- 제작 자산, Scene, 클래스에 `Recovery` 접두어를 붙이지 않는다.
- Missing Script를 바로 제거하지 않는다. YAML script GUID, `.meta`, 원본 코드와 DLL을 먼저 조사한다.
- 요청 없이 `git commit`, `git pull`, `git push` 또는 Unity 빌드를 실행하지 않는다.

## Scene 복원 절대 원칙

- 원본 근거 없이 Scene의 화면 구성, UI, 배경, 오브젝트, 문구, 버튼, 전환, 애니메이션 또는 연출을 임의로 만들거나 배치하지 않는다.
- 허용 근거는 원본 Scene/Prefab YAML, 유효한 GUID와 자산, 원본 코드/DLL, 원본 실행 이미지·영상, 또는 사용자가 직접 확인해 준 동작뿐이다.
- 근거가 불완전하면 화면을 추정해 완성하지 않는다. 조사 결과와 누락 항목만 기록하고, 해당 요소는 비활성 또는 미구현 상태로 둔다.
- `Estimated`와 `Placeholder`는 임의 제작 허가가 아니다. Scene 화면에 넣어야 할 경우 사용자의 명시적 승인을 먼저 받는다.
- 참조 이미지를 전체 화면 배경으로 붙여 복원을 대신하지 않는다. 이미지 속 요소를 분리할 근거가 부족하면 구현을 보류한다.
- 기존 복원 Scene도 위 기준을 충족하지 못하면 완료 상태를 취소하고 재조사 대상으로 되돌린다.
- Scene을 생성·수정하기 전에 해당 Scene의 근거 목록, hierarchy, 주요 RectTransform/Transform, 상태 전환을 task 문서에 먼저 확정한다.
- 검증은 “Unity에서 열린다”와 “원본 화면·동작을 복원했다”를 구분한다. 컴파일 및 전환 성공만으로 시각 복원 완료를 선언하지 않는다.

## 작업 방식

- 실제 구현은 `tasks/00_TEMPLATE.md` 형식을 따라 task 문서에 범위와 결과를 기록한다.
- 복구는 Scene 단위로 진행하고, 해당 Scene에 필요한 코드와 자산만 함께 가져온다.
- Scene 구현은 근거표가 승인되거나 충분히 확정된 요소부터 진행한다.
- 상태 전환과 추정에는 근거를 기록한다.
- 작업 완료 후 해당 task와 `CURRENT_STATE.md`를 갱신한다.
- 원본과 무관한 리팩터링, 패키지 업그레이드, Addressables/ECS/DI 전환은 기능 복원과 분리한다.

## 검증

- 정적 검증과 Unity compile/import, EditMode/PlayMode, 수동 검증 결과를 구분해 기록한다.
- Unity에서 확인하지 않은 동작을 정상이라고 단정하지 않는다.
- 하드웨어와 결제 장치가 없어도 테스트 진입 경로가 중단되지 않도록 simulator/adapter 경계를 유지한다.
