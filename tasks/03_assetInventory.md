# 에셋 및 GUID Inventory

## 상태

- 조사 중

## 복원 수준 목표

- R1

## 목표

- 남아 있는 에셋의 재사용 가능성을 분류한다.
- `.meta`, GUID, YAML 참조, Missing Script, Shader/Animator 누락을 전수 조사한다.

## 현재 결과

- 전체 파일 17,539개
- `.meta` 8,651개
- 파일 기준 missing `.meta` 237개
- orphan `.meta` 0개
- Scene 15개, Build Settings 활성 Scene 3개

## 완료 조건

- [ ] 확장자/경로/GUID/상태 inventory 생성
- [ ] 중복 GUID 목록 생성
- [ ] Scene/Prefab의 script GUID 목록 생성
- [ ] Missing Script 후보와 DLL/C# 타입 후보 연결
- [ ] 누락 Shader/Animator/Audio/TMP 참조 목록 생성
- [ ] P0 vertical slice 에셋 선정
- [ ] Placeholder 필요 목록 작성

## 주의

- Missing Script를 삭제하지 않는다.
- `.meta`를 임의 생성하지 않는다.
- 이름이 해시인 에셋을 용도 추정만으로 변경하지 않는다.

