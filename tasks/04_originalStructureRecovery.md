# 04. 원본 구조 조사와 복원 기반

## 상태

- 완료 (R1 구조 지도 작성)

## 목표

- 타입별로 평탄화된 `ExportedProject/Assets`에서 원래 게임의 논리 구조와 참조 관계를 조사한다.
- 전체를 한꺼번에 가져오지 않고 Scene 단위 복원에 필요한 근거를 제공한다.
- 실제 채택 파일은 정상 `Assets` 구조에 배치한다.

## 확인 결과

- 원본은 `Texture2D`, `Mesh`, `Material`, `MonoBehaviour` 등 타입 중심으로 평탄화된 export다.
- `StateManager`, `IStateinterface`, `FishGameManager`, `StageManager`, `DataManager`, `RodCont`, `TensionCont`, `MainObjectCont`의 C# 선언과 DLL 공개 member 이름이 대응한다.
- 원본 Build Settings의 이름 있는 Scene은 `Title`, `LoadingScene`, `LastScene`이다.
- `StageData._scene_number`와 `AdsManager._assets`를 대조해 선택 Scene 1개와 Gameplay Scene 11개의 역할을 매핑했다.
- 자세한 결과는 `Docs/ORIGINAL_STRUCTURE_MAP.md`에 기록했다.

## 기본 배치 정책

```text
UnityProject/Assets/
├─ Animations/
├─ Art/
├─ Audio/
├─ Materials/
├─ Models/
├─ Plugins/
├─ Prefabs/
├─ Scenes/
├─ ScriptableObjects/
├─ Scripts/
├─ ThirdParty/
└─ _ReferenceTemp/        # 선택적 임시 조사/변환 자료
```

1. `ExportedProject/`와 `AuxiliaryFiles/`는 계속 읽기 전용으로 유지한다.
2. 채택한 파일은 용도에 맞는 정상 경로로 직접 가져온다.
3. 유효한 원본 `.meta`가 있으면 자산과 함께 이동해 GUID를 보존한다.
4. 비교나 변환을 위해 Unity 내부 복사본이 필요한 경우에만 `_ReferenceTemp`를 사용한다.
5. 제품 Scene, Prefab, Script가 `_ReferenceTemp`에 의존하면 안 된다.
6. 출처와 `Original`, `Reconstructed`, `Estimated`, `Placeholder` 상태는 task와 문서에 기록한다.
7. 빌드된 Addressables bundle을 소스 자산처럼 가져오지 않는다.

## 구조 조사 영역

- 핵심 코드 provenance와 GUID 참조 지도
- Crest, DOTween, Filo, MK Glow, ParticleImage, TMP, URP, Addressables, Enviro 등 외부 경계
- 데이터 모델과 Resources/JSON/TextAsset/ScriptableObject 연결
- Title, Loading, Last, 선택, Gameplay Scene 역할
- Casting → Wait → Bite → Hooking → Fight/Holding → Finish/Catch 상태 흐름
- Serial, IMU, Reel Motor, Card 코드와 simulator/adapter 경계

## 완료 조건

- [x] 타입별 export 구조 확인
- [x] 핵심 8개 코드의 DLL/C# signature 및 GUID 참조 지도 작성
- [x] Title/Loading/Last/선택/Gameplay Scene 역할 분류
- [x] Gameplay Scene 11개와 StageData 지역명 연결
- [x] Loading Scene 직접 GUID와 runtime 결합 조사
- [x] 이후 실제 복원을 Scene 단위 `05` task로 이관
- [x] 정상 Assets 구조와 임시 참조 폴더 정책 확정

## 위험과 되돌리기

- 원본 소스와 DLL 추출 결과가 다를 수 있으므로 provenance를 계속 기록한다.
- 외부 패키지는 버전과 라이선스를 확인하기 전 설치하지 않는다.
- 각 Scene 변경은 독립적으로 기록하고, 문제가 생기면 해당 Scene과 그때 추가한 의존 파일만 제거한다.
- 원본 증거 폴더는 어떤 경우에도 변경하지 않는다.

