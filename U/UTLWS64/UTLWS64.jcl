#UTLWS64.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWS64.LWS64R1
	then
		rm ${reportdir}/ULWS64.LWS64R1
fi

if test -a /sas/whse/progrevw/ULWS64.LWS64R2
   then
        rm /sas/whse/progrevw/ULWS64.LWS64R2
fi

if test -a ${reportdir}/ULWS64.LWS64RZ
   then
        rm ${reportdir}/ULWS64.LWS64RZ
fi

# run the program

sas ${codedir}/UTLWS64.sas -log ${reportdir}/ULWS64.LWS64R1  -mautosource
