Add player dialog:
1. ~~User has to click away from player name before add player becomes clickable. Fix by using on text changed events instead.~~ FIXED - Added Immediate="true" to text field
2. ~~Play style preference - open should be default~~ FIXED - Changed default to Open
3. ~~Gender - only male & female, there should be no other option.~~ FIXED - UI now only shows Male/Female options

Navigation:
~~There is no easy way to go back from player management. We should use breadcrumbs and provide easy navigation to and from different areas of the site (Site wide)~~ FIXED - Breadcrumbs added site-wide

Add Session:
~~datetime picker is rendering the same width as the input, meaning it doesn't show correctly. Same for time~~ FIXED - Changed to PickerVariant.Dialog for proper display

Active Session:
~~BROKEN: When a match is put on a court, the component should be updated to show who is on the court, rather than keeping it as "still available". This now means there is no way to actively end a match.~~ FIXED - Added StateHasChanged() calls after LoadData() to ensure UI updates
