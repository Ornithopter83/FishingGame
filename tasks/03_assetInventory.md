# 03. 에셋 및 GUID Inventory

## 상태

- R1 완료

## 목표

- 남아 있는 에셋의 재사용 가능성을 분류하고 `.meta`, GUID, YAML 참조, Missing Script 후보를 전수 조사한다.

## 결과

- 비-meta 파일 8,888개와 meta 8,651개를 inventory에 기록
- meta 누락 파일 237개, orphan meta 0개
- 고유 GUID 8,651개, 중복 GUID 0개
- YAML GUID 참조 73,209건 조사
- Scene/Prefab script 참조 16,048건 조사, 미해결 script 참조 0건
- 해소되지 않은 고유 파일/GUID/property 조합 73건을 별도 기록
- 누락 Shader, Animator/Animation, Audio, TMP/Font 속성 후보는 현재 감사 기준 0건
- P0 수직 조각과 Placeholder 후보 선정

## 산출물

- `Docs/ASSET_GUID_INVENTORY.csv`: 경로, 확장자, 크기, meta 상태, GUID
- `Docs/SCENE_PREFAB_SCRIPT_GUIDS.csv`: Scene/Prefab별 script GUID와 해소 경로
- `Docs/UNRESOLVED_ASSET_REFERENCES.csv`: 해소되지 않은 YAML 참조와 property
- `Docs/ASSET_REFERENCE_AUDIT.md`: 집계, P0 선정, Placeholder 후보
- `Tools/Analyze-SourceAssets.ps1`: 재생성 도구

## 완료 조건

- [x] 확장자/경로/GUID/상태 inventory 생성
- [x] 중복 GUID 목록 생성
- [x] Scene/Prefab의 script GUID 목록 생성
- [x] Missing Script 후보와 DLL/C# 타입 후보 연결
- [x] 누락 Shader/Animator/Audio/TMP 참조 후보 목록 생성
- [x] P0 vertical slice 에셋 선정
- [x] Placeholder 필요 목록 작성

## 주의

- 해소되지 않은 73건은 대부분 texture/noise 계열 속성이며 package 파일, export 누락 또는 실제 Missing Reference가 섞였을 수 있다.
- Missing Script나 참조를 자동 삭제하지 않는다.
- `.meta`를 임의 생성하지 않는다.

