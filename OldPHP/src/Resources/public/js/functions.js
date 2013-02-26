$(document).ready(function(){
	$('.gifts li').click(function(){
		if(!$(this).hasClass('checked')){
			$('.gifts li').removeClass('checked');
			$(this).addClass('checked');
			$(this).parent().next().val(parseInt($(this).attr('rel')));
		}
		if($('#step2').hasClass('disabled')) {
			$('#step2').removeClass('disabled');
			$('.j-autocomplete input').removeAttr('disabled');
		}
		if ($('#reciprocal').length > 0) {
			$('#step3').removeClass('disabled');
                        $('#policy').removeClass('disabled');
                        $('#policy-box').removeAttr('disabled');                        
			$('.j-textarea').removeClass('disabled-textarea');
                        $('.j-textarea').removeClass('disabled');
			$('.j-textarea textarea').removeAttr('disabled');
		}
	});
	
	$('#goto-paypal').click(function(){
		if(!$(this).hasClass('disabled')){
                        var message;
                        if ($('#message').val() == $('#message').attr('rel')) {
                            message = '';
                        } else {
                            message = $('#message').val();
                        }
                        var data = 'friend=' + $('#friend-id').val() + '&item=' + $('#gift').val() + '&message=' + message; 
                        $.ajax({
                            type: "POST",
			    url : window.location,
                            data: data,
                            success: function (html) {
                                var dg = new PAYPAL.apps.DGFlow();
                                dg.startFlow(html);
                                $('#PPDGFrame .mask').remove();
                            }
                        });
			$('.overlay').show();
		}
		return false;
	});
        
        $('#policy-box').click(function () {
            if ($(this).attr('checked') == 'checked') {
                $('#goto-paypal').removeClass('disabled'); 
            } else {
                $('#goto-paypal').addClass('disabled'); 
            }
        });
	
	$('.j-popup-close').click(function(){
		$('.popup').hide();
		$('.overlay').hide();
		return true;
	});
        
        $('.popup').each(function() {
            $(this).hide();
        });
        
        $('.overlay').each(function() {
            $(this).hide();
        });
	
	$('#reciprocal').each(function() {
		
	});
	
    // Custom text field
    $('.j-textfield').each (function (){
        new TextField (this);
    });
	
    // Custom textarea
    $('.j-textarea').each (function (){
        new Textarea (this);
    });
	
    // Custom autocomplete
    $('.j-autocomplete').each (function (){
        new ACField (this);
    });
    
    $('#get-gift').click(function () {
        var email = $('#email').val();
        if (email != $('#email').attr('rel')) {
            $('.overlay').show();
            $('.overlay').html('<div class="waiting-notice">Requesting for transaction id...</div>');
            var href = window.location.toString();
            var giftHash = href.substring(href.lastIndexOf('getgift/'), href.length -1);
            $.ajax({
                type: 'POST',
		url: window.location,
                data: 'email=' + email,
                success: function (html) {
                    window.successGiftReceive(html);
                }
            });
        } else {
            alert('Plese, enter valid email');
        }
        
        return false;
    });
});


window.successGiftSend = function(response) {
    var data = $.parseJSON(response);
    $('#corellation-id').html(data.corellationId);
    $('#friend-name').html(data.friendName);
    $('.overlay').html('');
    $('.popup').show();
};

window.rejectGiftSend = function() {
    $('#PPDGFrame').remove();
    $('.overlay').hide();
}

window.successGiftReceive = function(response) {
    var data = $.parseJSON(response);
    if (data.paykey == 'fail') {
        $('.overlay').html('<div class="waiting-notice">There was a problem to transfer money to your account. Please, check email that you entered and try again...</div>');
        window.interval = window.setInterval(function () {
            $('.overlay').hide();
            clearInterval(window.interval);
        }, 10000);
        return;
    }    
    $('#corellation-id').html(data.paykey);
    $('.popup').show();
};

/* Custom textarea */
function Textarea (container){
    var _this = this;
    
    this.container = $(container);
	this.textarea = this.container.find('textarea');

	this.placeholder = this.textarea.attr('rel');
	this.textarea.addClass('placeholder');
	this.textarea.attr('disabled', 'disabled');
	
	this.textarea.val(this.placeholder);
	
	this.textarea.focus(function(){
		if($(this).val()==_this.placeholder){
			$(this).val('');
			$(this).removeClass('placeholder');
		}
	});
	this.textarea.blur(function(){
		if($(this).val()==''){
			$(this).val(_this.placeholder);
			$(this).addClass('placeholder');
		}
	});
}
/* /Custom text field */

/* Custom text field */
function TextField (container){
    var _this = this;
    
    this.className = $(container).attr ('class');
    this.container = $(container);
    
    this.container.wrap ('<div class="text-field-container ' + this.className + '"><div></div></div>');
	this.placeholder = this.container.attr('rel');
	this.container.addClass('placeholder');
	this.container.val(this.placeholder);
	
	this.container.focus(function(){
		if($(this).val()==_this.placeholder){
			$(this).val('');
			$(this).removeClass('placeholder');
		}
	});
	this.container.blur(function(){
		if($(this).val()==''){
			$(this).val(_this.placeholder);
			$(this).addClass('placeholder');
		}
	});
}
/* /Custom text field */

/* Custom autocomplete */
function ACField (container){
    var _this = this;
    
    this.container = $(container);
    this.className = this.container.attr ('class');
	this.placeholder = this.container.attr('rel');
	this.container.addClass('placeholder');
	this.container.attr('disabled', 'disabled');
	    
    this.container.wrap ('<div class="text-acfield ' + this.className + '"><div></div></div>');
	
	this.container.autocomplete({
		source: window.searchFriendUrl,
		minLength: 2,
		select: function( event, ui ) {
			$('#person').html("<div><img src='" + ui.item.label + "' /> <span>" + ui.item.value + "</span> <div class='del'></div></div>");
                        $('#friend-id').val(ui.item.id);
			$('#step3').removeClass('disabled');
                        $('#policy').removeClass('disabled');
                        $('#policy-box').removeAttr('disabled');                        
			$('.j-textarea').removeClass('disabled-textarea');
			$('.j-textarea textarea').removeAttr('disabled');                        
		}
	}).data( "autocomplete" )._renderItem = function( ul, item ) {
		return $( "<li></li>" )
			.data( "item.autocomplete", item )
			.append( "<a><img src='" + item.label + "' /> <span>" + item.value + "</span></a>" )
			.appendTo( ul );
	}
	
	this.container.val(this.placeholder);
	
	this.container.focus(function(){
		if($(this).val()==_this.placeholder){
			$(this).val('');
			$(this).removeClass('placeholder');
		}
	});
	this.container.blur(function(){
		if($(this).val()==''){
			$(this).val(_this.placeholder);
			$(this).addClass('placeholder');
		}
	});
	
	$('#person .del').live('click', function(){
		$('#person').html('');
		_this.container.val('');
		_this.container.trigger('blur');
		$('#step3').addClass('disabled');
		$('.j-textarea').addClass('disabled-textarea');
		$('.j-textarea textarea').attr('disabled', 'disblaed');
                $('#policy').addClass('disabled');
                $('#policy-box').removeAttr('checked');
                $('#policy-box').attr('disabled', true);
		$('#goto-paypal').addClass('disabled');
	});
}
/* /Custom autocomplete */
