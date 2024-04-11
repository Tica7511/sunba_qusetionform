<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DormitoryRegister.aspx.cs" Inherits="WebPage_DormitoryRegister" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript" src="../page_js/DormitoryRegister.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container-xxl mb-3">
		<form id="dataForm" name="dataForm">
			<div class="border border-dark p-2 mt-1">
				<div class="ochiform TitleLength08">
					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">申請人姓名</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control" type="text" id="d_name" name="d_name" value="<%= empName %>" disabled="disabled" />
								</div>
							</div>
						</div>
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label" for="Name">申請類別</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<select class="form-select" aria-label="Default select example" id="d_type" name="d_type"></select>
								</div>
							</div>
						</div>
					</div>

					<div class="ItemForA">
						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label" for="formA1">戶籍謄本</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<input class="form-control" type="file" id="d_file" name="d_file" />
							</div>
						</div>

						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">住宿辦法</div>
							<div class="col-md-auto flex-grow-1">
								<input class="form-check-input" type="checkbox" id="cb_check">
								<label class="form-check-label" for="eq01">
									我已閱讀完宿舍管理要點並確認符合住宿資格
								</label>
								<textarea rows="5" class="form-control">
一、本公司豐德電廠宿舍（以下稱本宿舍）係為解決同仁之駐勤住宿問題而設置。
二、本宿舍僅供居住距辦公地點在50公里以上，無法便利通勤上班，或因工作需要，必需長期留守駐勤由住宿者提出申請，經主管核定由行管部統一安排住宿。
三、本宿舍以借貸方式配借，為二人共同使用一間房及附設之盥洗設備。
四、本宿舍僅供本人經常性住宿使用，不得攜眷或留滯親友同宿。
五、本宿舍之管理單位為行管部，為利於管理及維護，由行管部指派一人駐舍監管。
六、本宿舍借貸優先順序：
　（一）、填具宿舍調換申請書之現住人員。
　（二）、已填具借貸宿舍申請書，並核可借貸者。
七、配借申請方式：
　（一）、填具借貸宿舍申請書，並核可借貸者：
⒈向行管部取具填寫借住宿舍申請書（附件一），經行管部於接獲申請書後核定借貸資格；如申請人數超出宿舍可配借人數致不敷分配時，則以申請先後順序，依序遞補。
⒉取得借貸資格者行管部連絡申請人辦理配借事宜，被通知者可於通知名單中自行覓妥另一員，以二人為單位，向行管部申請配借宿舍。
（二）、填具宿舍調換申請書之現住人員：
⒈向行管部取具填寫宿舍調換申請書（附件二），經行管部核定借貸資格，如申請人數超出宿舍可配借人數致不敷分配時，則以抽籤決定該次配借順序，依序遞補。
⒉取得借貸資格者，行管部以通知單分別通知申請人配用事宜。首次借貸分配者可於通知名單或現住人員中，自行覓妥另一員，以二人為單位，向行管部申請調換配借寢室，惟以一次為限。
　（三）、核准配借之宿舍，行管部先作必要檢修後，限一星期內遷入，否則視為棄權，所配借之宿舍即予收回，不得要求保留或再配借。　
（四）、配借宿舍之同仁於遷入前應先填具切結書（附表三）並與行管部簽訂借住契約（附件四）。
八、交還宿及舍期限：
（一）、借住人因離職、留職(資)停薪、調職、免職、退休、違反借貸契約或違反本管理要點時應交還宿舍。

（二）、離職、留職(資)停薪、調職、免職、違反借貸契約或違反本管理要點者交還宿舍期限為五天，退休者交還宿舍期限為一星期。
九、配借宿舍應繳（扣）收費用如左：
（一）、清潔管理費每人每月  元。
 （二）、點交財產設備及鑰匙時，應繳交一仟元作為保證金，於交還宿舍期限點還各式器材及鑰匙後，無息發還。
十、宿舍之停車場（位）統由電廠分配管理使用，借住人不得隨意停放。
十一、借住人應善良管理使用公司提供之各項設備，交還宿舍時應依財產設備清單（附件五）點還管理部門，如有短缺或損壞應負賠償責任。
十二、房舍異動前，由管理部門作必要檢修後，點交予新配借使用人，借住人應於進住時點清並簽署財產設備清單點交表單。
十三、借住人應盡善良管理責任，公司提供公用設備如電冰箱、洗衣機、烘衣機及公共區域之照明等設備由公司負責維護保養，分戶房間內之各項設備，使用不當引起之損壞，應由借住人自行負擔維修費用。
十四、不得私相授受，供非配借人居住，或本人不經常住宿使用，或利用他人名義而僅一人借住。
十五、房間騰空時，現住人得洽行管部按申請登記先後順序調整配住宿舍，惟以一次為原則，或另由行管部按申請登記先後順序配借，現住人不得異議。
十六、宿舍內不得私自接用公共水電，或於房間內使用超出用電負荷之電器用品。
十七、宿舍內嚴禁存放違禁品，如經查覺報請治安機關處理。
十八、宿舍房間內及其衛浴室、陽台由使用者自行清潔，其廢棄物及垃圾應分類置於各樓設置垃圾桶內；樓梯、走道、公共陽台及屋頂等公共區域由管理部門負責派員清潔。
十九、樓梯走道等公共區域不得擺設堆放私人物品。
二十、借住人雙方應互相溝通作息時間及生活習慣，彼此體諒，以維持和諧關係。

二十一、借住人雙方應養成良好生活習慣，不得違反善良風俗，或以其他方式使另一方不堪同房居住。
二十二、借住人雙方如因個人生活習性不佳或其他原因而產生嫌隙時，應由宿舍管理委員會及行管部仲裁，經勸止不聽者，簽核後即終止借貸。
二十三、宿舍內外應保持整潔，以維護公共衛生。
二十四、進出宿舍應隨手關門，以防外人進入，門禁安全由使用人共同維護。
二十五、宿舍內應保持寧靜不得喧嘩，嚴禁酗酒､賭博及其他不正當之行為。
二十六、預颱風、地震、火災或其他意外情事發生時，應配合管理人員之引導，採取緊急措施，維護公共安全。
二十七、宿舍借住人，違反本要點有關規定，廠方得視情節輕重收回其所借之宿舍。
二十八、本宿舍因公司政策另有他用，必須騰空遷讓時，公司得隨時終止借貸收回宿舍。
二十九、本要點奉核定後實施，修正時亦同。
                  </textarea>
							</div>
						</div>

					</div>
					<div class="ItemForB" style="display: none;">
						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">住宿起訖</div>
							<div class="col-md-auto flex-grow-1">
								<div class="d-flex align-items-center flex-nowrap">
									<div class="flex-grow-1 me-1">
										<input class="form-control" type="date" id="d_startday" name="d_startday" />
									</div>
									<div class="text-center">~</div>
									<div class="flex-grow-1 ms-1">
										<input class="form-control" type="date" id="d_endday" name="d_endday" />
									</div>
								</div>
							</div>
						</div>

						<div class="row mt-1 flex-md-nowrap align-items-center OchiRow">
							<div class="col-md-auto TitleSetWidth text-md-end">
								<label class="form-label" for="reson">申請事由</label>
							</div>
							<div class="col-md-auto flex-grow-1">
								<input class="form-control" type="text" id="d_reason" name="d_reason" />
							</div>
						</div>
					</div>

					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">聯絡電話(手機)</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control num" type="text" id="d_tel" name="d_tel" />
								</div>
							</div>
						</div>
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">血型</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control" type="text" id="d_bloodtype" name="d_bloodtype">
								</div>
							</div>
						</div>
					</div>

					<div class="row gy-2 mt-1 align-items-center OchiRow">
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">緊急聯絡人</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control" type="text" id="d_emergency_contact" name="d_emergency_contact" />
								</div>
							</div>
						</div>
						<div class="col-md-6">
							<div class="row flex-md-nowrap align-items-center">
								<div class="col-md-auto TitleSetWidth text-md-end">
									<label class="form-label">緊急聯絡人電話</label>
								</div>
								<div class="col-md-auto flex-grow-1">
									<input class="form-control num" type="text" id="d_emergency_tel" name="d_emergency_tel" />
								</div>
							</div>
						</div>
					</div>
				</div>

				<div class="text-end mt-2">
					<a href="javascript:void(0);" id="savebtn" class="btn btn-primary text-nowrap btn-sm">送出</a>
				</div>
			</div>
		</form>
		<div id="errMsg" style="color:red;"></div>
	</div>
</asp:Content>

